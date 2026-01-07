using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaGestaoIA.DataContext;
using PlataformaGestaoIA.Models;
using PlataformaGestaoIA.ViewModel;
using Microsoft.AspNetCore.Authorization;
using PlataformaGestaoIA.Attributes;

namespace PlataformaGestaoIA.Controllers
{
    [ApiController]
    public class SkillController : ControllerBase
    {
        private readonly PrincipalDataContext _context;

        public SkillController(PrincipalDataContext context)
        {
            _context = context;
        }
        [Authorize]
        [HttpPost("api/v1/skills")]
        [ProducesResponseType(typeof(ResultViewModel<Skill>), 200)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
        public async Task<IActionResult> Post([FromBody] Skill skill)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                skill.Tag = skill.Name.Replace("@", "-").Replace(".", "-");
                await _context.Skills.AddAsync(skill);
                await _context.SaveChangesAsync();

                return Ok(new ResultViewModel<Skill>(skill));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("Falha interna no servidor."));
            }
        }

        [ApiKey]
        [HttpGet("api/v1/skills/{id:int}")]
        [ProducesResponseType(typeof(ResultViewModel<Skill>), 200)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
        public async Task<IActionResult> GetById(int id)
        {
            var skill = await _context.Skills.FindAsync(id);

            if (skill == null)
                return NotFound(new ResultViewModel<Skill>("Habilidade n�o encontrada."));

            return Ok(new ResultViewModel<Skill>(skill));
        }
        //[ApiKey]
        [HttpGet("api/v1/skills")]
        [ProducesResponseType(typeof(ResultViewModel<List<Skill>>), 200)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
        public async Task<IActionResult> GetAll()
        {
            var skills = await _context.Skills.ToListAsync();
            return Ok(new ResultViewModel<List<Skill>>(skills));
        }
        [Authorize]
        [HttpPut("api/v1/skills/{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] Skill skill)
        {
            var existingSkill = await _context.Skills.FindAsync(id);

            if (existingSkill == null)
                return NotFound(new ResultViewModel<string>("Habilidade n�o encontrada."));

            existingSkill.Name = skill.Name;
            existingSkill.IsSoftSkill = skill.IsSoftSkill;

            try
            {
                _context.Skills.Update(existingSkill);
                await _context.SaveChangesAsync();

                return Ok(new ResultViewModel<Skill>(existingSkill));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("Falha ao atualizar a habilidade."));
            }
        }
        [Authorize]
        [HttpDelete("api/v1/skills/{id:int}")]
        [ProducesResponseType(typeof(ResultViewModel<string>), 200)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
        public async Task<IActionResult> Delete(int id)
        {
            var skill = await _context.Skills.FindAsync(id);

            if (skill == null)
                return NotFound(new ResultViewModel<string>("Habilidade n�o encontrada."));

            try
            {
                _context.Skills.Remove(skill);
                await _context.SaveChangesAsync();

                return Ok(new ResultViewModel<string>("Habilidade exclu�da com sucesso"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("Falha ao excluir a habilidade."));
            }
        }
    }
}