using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaGestaoIA.DataContext;
using PlataformaGestaoIA.Extensions;
using PlataformaGestaoIA.Models;
using PlataformaGestaoIA.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlataformaGestaoIA.Controllers
{
    [ApiController]
    [Route("api/v1/projects")]
    public class ProjectController : ControllerBase
    {
        private readonly PrincipalDataContext _context;

        public ProjectController(PrincipalDataContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResultViewModel<Project>), 200)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
        public async Task<IActionResult> PostAsync([FromBody] EditorProjectViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

            try
            {  
                var project = new Project
                {
                    Description = model.Description,
                    InternalCode = model.InternalCode,
                };

                // Adicionar o novo projeto ao contexto e salvar as mudanças
                await _context.Projects.AddAsync(project);
                await _context.SaveChangesAsync();

                return Ok(new ResultViewModel<Project>(project));
            }
            catch (DbUpdateException)
            {
                // Lidar com exceções de violação de chave única
                return StatusCode(400, new ResultViewModel<string>("05X99 - Este projeto já está cadastrado"));
            }
            catch (Exception)
            {
                // Lidar com outras exceções inesperadas
                return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna no servidor"));
            }
        }

        [HttpPut("project-skill/{id}")]
        [ProducesResponseType(typeof(ResultViewModel<Project>), 200)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
        public async Task<IActionResult> PutProjectSkillAsync(int id, [FromBody] EditorProjectSkillViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

            try
            {
                // Carregar o projeto com os ProjectSkills associados
                var project = await _context.Projects
                    .Include(p => p.ProjectSkill) // Carrega os ProjectSkill relacionados
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (project == null)
                    return NotFound(new ResultViewModel<Project>("Conteúdo não encontrado"));

                // Remover todos os ProjectSkill associados ao projeto
                project.ProjectSkill.Clear();

                // Extrair IDs das habilidades do modelo
                var idList = model.SkillLevel.Select(ps => ps.SkillId).ToList();

                // Buscar habilidades correspondentes no banco de dados
                var skillsArray = await _context.Skills
                    .Where(skill => idList.Contains(skill.Id))
                    .AsTracking()
                    .ToArrayAsync();

                // Criar nova lista de ProjectSkill
                var newProjectSkills = skillsArray.Select(skill => new ProjectSkill
                {
                    Skill = skill,
                    Level = model.SkillLevel.FirstOrDefault(ps => ps.SkillId == skill.Id)?.Level ?? 0,
                }).ToList();

                // Adicionar os novos ProjectSkill ao projeto
                project.ProjectSkill = newProjectSkills;

                // Atualizar o projeto no contexto e salvar as mudanças
                _context.Projects.Update(project);
                await _context.SaveChangesAsync();

                return Ok(new ResultViewModel<Project>(project));
            }
            catch (DbUpdateException)
            {
                // Lidar com exceções de violação de chave única
                return StatusCode(400, new ResultViewModel<string>("05X99 - Este projeto já está cadastrado"));
            }
            catch (Exception)
            {
                // Lidar com outras exceções inesperadas
                return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna no servidor"));
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResultViewModel<Project>), 200)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var project = await _context
                    .Projects
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (project == null)
                    return NotFound(new ResultViewModel<Project>("Projeto não encontrado"));

                return Ok(new ResultViewModel<Project>(project));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("Falha interna no servidor"));
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResultViewModel<List<Project>>), 200)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var projects = await _context.Projects.ToListAsync();
                return Ok(new ResultViewModel<List<Project>>(projects));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna no servidor"));
            }
        }

        [HttpGet("project-skill/{id}")]
        [ProducesResponseType(typeof(ResultViewModel<Project>), 200)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
        public async Task<IActionResult> GetProjectSkilltAsync(int id)
        {
            try
            {
                var projects = await _context.Projects.Include(sr => sr.ProjectSkill).ThenInclude(ss => ss.Skill).FirstOrDefaultAsync(x => x.Id == id);
                return Ok(new ResultViewModel<Project>(projects));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna no servidor"));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] Project model)
        {
            try
            {
                var project = await _context
                    .Projects
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (project == null)
                    return NotFound(new ResultViewModel<Project>("Conteúdo não encontrado"));

                project.Description = model.Description;
                project.InternalCode = model.InternalCode;

                _context.Projects.Update(project);
                await _context.SaveChangesAsync();

                return Ok(new ResultViewModel<Project>(project));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Project>("05XE8 - Não foi possível alterar o projeto"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<Project>("05X11 - Falha interna no servidor"));
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResultViewModel<Project>), 200)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var project = await _context
                    .Projects
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (project == null)
                    return NotFound(new ResultViewModel<Project>("Conteúdo não encontrado"));

                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();

                return Ok(new ResultViewModel<string>("Projeto excluído com sucesso"));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Project>("05XE7 - Não foi possível excluir o projeto"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<Project>("05X12 - Falha interna no servidor"));
            }
        }
    }
}