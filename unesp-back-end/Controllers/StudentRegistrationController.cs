using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaGestaoIA.DataContext;
using PlataformaGestaoIA.Extensions;
using PlataformaGestaoIA.Models;
using PlataformaGestaoIA.ViewModel;
using PlataformaGestaoIA.Controllers.Functions;
using System;
using System.Text.Json;

namespace PlataformaGestaoIA.Controllers
{
    //[Authorize]
    [ApiController]
    public class StudentRegistrationController : ControllerBase
    {
        [HttpPost("api/v1/student-registrations")]
        public async Task<IActionResult> Post(
          [FromForm] StudentRegistration studentRegistration,
          [FromForm] IFormFile file_registration,
          [FromForm] string studentRegistrationScore,
          [FromForm] string studentSkills,
          [FromServices] PrincipalDataContext context)
        {
            var generalConfig = await context.GeneralConfigs.FirstOrDefaultAsync();
            if (string.Equals(generalConfig?.Stage, "confirmacao", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new ResultViewModel<string>("Inscrições encerradas. A etapa de confirmação está ativa."));
            }

            var registrationScore = StudentRegistrationScoreControllerFunction.DeserializeStudentScore(studentRegistrationScore);
            var skillsTemp = StudentRegistrationSkillControllerFunction.DeserializeStudentSkills(studentSkills);

            Console.WriteLine("StudentSkills JSON recebido: " + studentRegistration.StudentSkills);
            Console.WriteLine("Tipo de StudentSkills: " + (studentRegistration.StudentSkills != null ? studentRegistration.StudentSkills.GetType().ToString() : "null"));

            if (!string.IsNullOrEmpty(studentRegistrationScore))
                studentRegistration.StudentRegistrationScore = registrationScore;
            
            if (!string.IsNullOrEmpty(studentSkills))
            {
                studentRegistration.StudentSkills = skillsTemp.Select(temp => new StudentRegistrationSkill
                {
                    Skill = new Skill
                    {
                        Name = temp.Skill.Name,
                        Tag = temp.Skill.Tag
                    },
                    Level = temp.Level
                }).ToList();
            }

            //Console.WriteLine("StudentRegistrationScore JSON recebido: " + studentRegistration.StudentRegistrationScore);
            Console.WriteLine("Tipo de StudentRegistrationScore: " + (studentRegistration.StudentRegistrationScore != null ? studentRegistration.StudentRegistrationScore.GetType().ToString() : "null"));

            var verifyResult = await StudentRegistrationControllerFunction.VerifySemester(studentRegistration, context);

            if (verifyResult.Exists)
            {
                studentRegistration = verifyResult.StudentRegistration;
            }

            try
            {
                if (file_registration != null && file_registration.Length > 0)
                {
                    Console.WriteLine($"Arquivo recebido: {file_registration.Length} bytes");

                    if (verifyResult.Exists)
                    {
                        context.StudentRegistrations.Update(studentRegistration);
                    }
                    else
                    {
                        await StudentRegistrationControllerFunction.TrackEntities(studentRegistration, context);
                        await context.StudentRegistrations.AddAsync(studentRegistration);
                    }

                    // Define a pasta onde os arquivos serão armazenados (dentro da aplicação)
                    var relativePath = Path.Combine("PrivateFiles", studentRegistration.Semester, studentRegistration.Student.RA);
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), relativePath);

                    // Garante que a pasta de destino existe
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Gera um nome único para o arquivo
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file_registration.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    // Salva o arquivo no diretório
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file_registration.CopyToAsync(stream);
                    }

                    // Salva apenas o caminho relativo no banco de dados
                    studentRegistration.FilePath = Path.Combine(relativePath, fileName);

                    await context.SaveChangesAsync();
                }

                return Ok(new ResultViewModel<StudentRegistration>(studentRegistration));
            }
            catch (DbUpdateException)
            {
                return StatusCode(400, new ResultViewModel<string>("05X99 - Registro de estudante já está cadastrado"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado: {ex.Message}");
                return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna no servidor"));
            }
        }


        [HttpGet("api/v1/student-registrations/{id:int}")]
        [ProducesResponseType(typeof(ResultViewModel<StudentRegistration>), 200)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
        public async Task<IActionResult> GetByIdAsync(
            [FromRoute] int id,
            [FromServices] PrincipalDataContext context)
        {
            try
            {
                var studentRegistration = await context.StudentRegistrations
                    .Include(sr => sr.Student)
                        .ThenInclude(ss => ss.CurrentCourse)
                    .Include(sr => sr.Student)
                        .ThenInclude(s => s.User)
                    .Include(sr => sr.StudentRegistrationScore)
                    .Include(sr => sr.StudentSkills)
                        .ThenInclude(ss => ss.Skill)
                    .FirstOrDefaultAsync(sr => sr.Id == id);

                if (studentRegistration == null)
                    return NotFound(new ResultViewModel<StudentRegistration>("Registro de estudante não encontrado"));

                return Ok(new ResultViewModel<StudentRegistration>(studentRegistration));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<StudentRegistration>("Falha interna no servidor"));
            }
        }

        [HttpGet("api/v1/student-registrations")]
        [ProducesResponseType(typeof(ResultViewModel<List<StudentRegistration>>), 200)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
        public async Task<IActionResult> GetAllAsync(
            [FromServices] PrincipalDataContext context,
            [FromQuery] int pageNumber = 1,  // Número da página (padrão: 1)
            [FromQuery] int pageSize = 10   // Tamanho da página (padrão: 10)
            )
        {
            try
            {
                pageSize = 100000;
                // Verifica o total de registros
                var totalRecords = await context.StudentRegistrations.CountAsync();

                // Calcula o número total de páginas
                var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                // Verifica se a página está dentro do intervalo válido
                if (pageNumber < 1 || pageNumber > totalPages)
                {
                    return BadRequest(new ResultViewModel<string>("Página fora do intervalo"));
                }

                // Consulta com a paginação
                var studentRegistrations = await context.StudentRegistrations
                    .Include(sr => sr.Student)
                        .ThenInclude(ss => ss.CurrentCourse)
                    .Include(sr => sr.Student)
                        .ThenInclude(s => s.User)
                    .Include(sr => sr.StudentRegistrationScore)
                    .Include(sr => sr.StudentSkills)
                        .ThenInclude(ss => ss.Skill)
                    .Skip((pageNumber - 1) * pageSize)  // Pula os registros anteriores
                    .Take(pageSize)  // Toma o número de registros especificado
                    .ToListAsync();

                // Retorna os registros e informações de paginação
                var result = new ResultViewModel<List<StudentRegistration>>(studentRegistrations)
                {
                    TotalRecords = totalRecords,
                    TotalPages = totalPages,
                    CurrentPage = pageNumber,
                    PageSize = pageSize
                };

                return Ok(result);
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna no servidor"));
            }
        }

        [HttpPut("api/v1/student-registrations/{id:int}")]
        [ProducesResponseType(typeof(ResultViewModel<StudentRegistration>), 200)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
        public async Task<IActionResult> PutAsync(
            [FromRoute] int id,
            [FromBody] StudentRegistration studentRegistration,
            [FromServices] PrincipalDataContext context)
        {
            try
            {
                var existingStudentRegistration = await context.StudentRegistrations
                    .Include(sr => sr.Student)
                      .ThenInclude(ss => ss.CurrentCourse)
                    .Include(sr => sr.Student)
                        .ThenInclude(s => s.User)
                    .Include(sr => sr.StudentRegistrationScore)
                    .Include(sr => sr.StudentSkills)
                    .ThenInclude(ss => ss.Skill)
                    .FirstOrDefaultAsync(sr => sr.Id == id);

                if (existingStudentRegistration == null)
                    return NotFound(new ResultViewModel<string>("Registro de estudante não encontrado"));

                existingStudentRegistration.Student = studentRegistration.Student;
                existingStudentRegistration.SkillsDescription = studentRegistration.SkillsDescription;
                existingStudentRegistration.RegistrationDate = studentRegistration.RegistrationDate;
                existingStudentRegistration.Subject = studentRegistration.Subject;
                existingStudentRegistration.ChoicePriority = studentRegistration.ChoicePriority;
                existingStudentRegistration.EnrolledInIndustry4_0 = studentRegistration.EnrolledInIndustry4_0;
                existingStudentRegistration.DoesNotMeetRequirements = studentRegistration.DoesNotMeetRequirements;
                existingStudentRegistration.FirstMeetingDate = studentRegistration.FirstMeetingDate;
                existingStudentRegistration.Semester = studentRegistration.Semester;
                existingStudentRegistration.StudentRegistrationScore = studentRegistration.StudentRegistrationScore;
                existingStudentRegistration.StudentSkills = studentRegistration.StudentSkills;

                context.StudentRegistrations.Update(existingStudentRegistration);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<StudentRegistration>(existingStudentRegistration));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<string>("05XE8 - Não foi possível alterar o registro de estudante"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("05X11 - Falha interna no servidor"));
            }
        }

        [HttpDelete("api/v1/student-registrations/{id:int}")]
        [ProducesResponseType(typeof(ResultViewModel<string>), 200)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] int id,
            [FromServices] PrincipalDataContext context)
        {
            try
            {
                var studentRegistration = await context.StudentRegistrations
                    .FirstOrDefaultAsync(sr => sr.Id == id);

                if (studentRegistration == null)
                    return NotFound(new ResultViewModel<string>("Registro de estudante não encontrado"));

                context.StudentRegistrations.Remove(studentRegistration);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<string>("Registro de estudante deletado com sucesso"));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<string>("05XE7 - Não foi possível excluir o registro de estudante"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("05X12 - Falha interna no servidor"));
            }
        }

        // Método GET para pegar o arquivo PDF associado ao StudentRegistration
        [HttpGet("api/v1/student-registrations/file/{id}")]
        [ProducesResponseType(typeof(FileResult), 200)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 404)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
        public async Task<IActionResult> GetFileAsync(
            [FromRoute] int id,  // ID do StudentRegistration
            [FromServices] PrincipalDataContext context)
        {
            try
            {
                var studentRegistration = await context.StudentRegistrations
                    .Include(sr => sr.Student)
                    .FirstOrDefaultAsync(sr => sr.Id == id);

                if (studentRegistration == null || string.IsNullOrEmpty(studentRegistration.FilePath))
                {
                    return NotFound(new ResultViewModel<string>("Arquivo não encontrado."));
                }

                var fullPath = $"{GeneralFunction.GetPdfDirectory()}\\{studentRegistration.FilePath}";

                // 2. Verifica se o arquivo existe no caminho especificado
                if (!System.IO.File.Exists(fullPath))
                {
                    return NotFound(new ResultViewModel<string>("Arquivo não encontrado no servidor."));
                }

                // 3. Lê o arquivo e retorna como FileResult
                var fileBytes = await System.IO.File.ReadAllBytesAsync(fullPath);
                return File(fileBytes, "application/pdf", Path.GetFileName(fullPath));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna no servidor"));
            }
    }

    [Authorize]
    [HttpGet("api/v1/files/{semester}/{ra}/{fileName}")]
    public async Task<IActionResult> GetFile(string semester, string ra, string fileName)
    {
        // Construa o caminho do arquivo
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "PrivateFiles", semester, ra, fileName);

        // Verifique se o arquivo existe
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound(new ResultViewModel<string>("Arquivo não encontrado"));
        }

        // Retorne o arquivo
        var fileStream = System.IO.File.OpenRead(filePath);
        return File(fileStream, "application/pdf", fileName);
    }
}
}
