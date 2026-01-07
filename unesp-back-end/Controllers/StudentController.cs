using PlataformaGestaoIA.Models;
using PlataformaGestaoIA.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaGestaoIA.DataContext;
using PlataformaGestaoIA.Extensions;
using PlataformaGestaoIA.Attributes;
using Microsoft.AspNetCore.Authorization;

namespace PlataformaGestaoIA.Controllers;

[ApiController]
[Authorize]
public class StudentController : ControllerBase
{
    [HttpPost("api/v1/students/")]
    [ProducesResponseType(typeof(ResultViewModel<Student>), 200)]
    [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
    [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
    public async Task<IActionResult> PostAsync(
        [FromBody] EditorStudentViewModel model,
        [FromServices] PrincipalDataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        var student = new Student
        {
            Name = model.Name,
            RA = model.RA,
            CPF = model.CPF,
            RG = model.RG,
            Cellphone = model.CellPhone,
            CurrentCourse = model.CurrentCourse
        };

        try
        {
            await context.Students.AddAsync(student);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Student>(student));
        }
        catch (DbUpdateException)
        {
            return StatusCode(400, new ResultViewModel<string>("05X99 - Este estudante j� est� cadastrado"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna no servidor"));
        }
    }

    [ProducesResponseType(typeof(ResultViewModel<Student>), 200)]
    [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
    [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
    [HttpGet("api/v1/students/{id:int}")]
    public async Task<IActionResult> GetByIdAsync(
            [FromRoute] int id,
            [FromServices] PrincipalDataContext context)
    {
        try
        {
            var student = await context
                .Students
                .Include(x => x.CurrentCourse)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (student == null)
                return NotFound(new ResultViewModel<Student>("Estudante n�o encontrado"));

            return Ok(new ResultViewModel<Student>(student));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<Student>("Falha interna no servidor"));
        }
    }

    [HttpGet("api/v1/students")]
    [ProducesResponseType(typeof(ResultViewModel<List<Student>>), 200)]
    [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
    [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
    public async Task<IActionResult> GetAsync(
            [FromServices] PrincipalDataContext context)
    {
        try
        {
            var students = await context.Students.Include(x => x.CurrentCourse).ToListAsync();
            return Ok(new ResultViewModel<List<Student>>(students));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<List<Student>>("05X04 - Falha interna no servidor"));
        }
    }

    [HttpPut("api/v1/students/{id:int}")]
    public async Task<IActionResult> PutAsync(
            [FromRoute] int id,
            [FromBody] EditorStudentViewModel model,
            [FromServices] PrincipalDataContext context)
    {
        try
        {
            var student = await context
                .Students
                .FirstOrDefaultAsync(x => x.Id == id);

            if (student == null)
                return NotFound(new ResultViewModel<Student>("Conte�do n�o encontrado"));

            student.Name = model.Name;
            student.RA = model.RA;

            context.Students.Update(student);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Student>(student));
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new ResultViewModel<Student>("05XE8 - N�o foi poss�vel alterar a categoria"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResultViewModel<Student>("05X11 - Falha interna no servidor"));
        }
    }

    [HttpDelete("api/v1/students/{id:int}")]
    [ProducesResponseType(typeof(ResultViewModel<Student>), 200)]
    [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
    [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
    public async Task<IActionResult> DeleteAsync(
            [FromRoute] int id,
            [FromServices] PrincipalDataContext context)
    {
        try
        {
            var student = await context
                .Students
                .FirstOrDefaultAsync(x => x.Id == id);

            if (student == null)
                return NotFound(new ResultViewModel<Student>("Conte�do n�o encontrado"));

            context.Students.Remove(student);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<string>("Estudante encontrado com sucesso"));
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new ResultViewModel<Student>("05XE7 - N�o foi poss�vel excluir o estudante"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResultViewModel<Student>("05X12 - Falha interna no servidor"));
        }
    }

}