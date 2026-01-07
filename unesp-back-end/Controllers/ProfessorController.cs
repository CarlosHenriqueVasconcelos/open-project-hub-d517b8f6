using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaGestaoIA.DataContext;
using PlataformaGestaoIA.Models;
using PlataformaGestaoIA.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlataformaGestaoIA.Attributes;
using Microsoft.AspNetCore.Authorization;

namespace PlataformaGestaoIA.Controllers
{
    [ApiController]
    [Authorize]
    public class ProfessorController : ControllerBase
    {
        private readonly PrincipalDataContext _context;

        public ProfessorController(PrincipalDataContext context)
        {
            _context = context;
        }

        [HttpPost("api/v1/professors")]
        [ProducesResponseType(typeof(ResultViewModel<Professor>), 200)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
        public async Task<IActionResult> Post([FromBody] Professor professor)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _context.Professors.AddAsync(professor);
                await _context.SaveChangesAsync();

                return Ok(new ResultViewModel<Professor>(professor));
            }
            catch (DbUpdateException)
            {
                return StatusCode(400, new ResultViewModel<string>("Este e-mail j� est� cadastrado."));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("Falha interna no servidor."));
            }
        }

        [HttpGet("api/v1/professors/{id:int}")]
        [ProducesResponseType(typeof(ResultViewModel<Professor>), 200)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
        public async Task<IActionResult> GetById(int id)
        {
            var professor = await _context.Professors.FindAsync(id);

            if (professor == null)
                return NotFound(new ResultViewModel<Professor>("Professor n�o encontrado."));

            return Ok(new ResultViewModel<Professor>(professor));
        }

        [HttpGet("api/v1/professors")]
        [ProducesResponseType(typeof(ResultViewModel<List<Professor>>), 200)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
        public async Task<IActionResult> GetAll()
        {
            var professors = await _context.Professors.ToListAsync();
            return Ok(new ResultViewModel<List<Professor>>(professors));
        }

        [HttpPut("api/v1/professors/{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] Professor professor)
        {
            if (id != professor.Id)
                return BadRequest(new ResultViewModel<string>("Id da rota n�o corresponde ao Id do professor."));

            var existingProfessor = await _context.Professors.FindAsync(id);

            if (existingProfessor == null)
                return NotFound(new ResultViewModel<string>("Professor n�o encontrado."));

            existingProfessor.Name = professor.Name;

            try
            {
                _context.Professors.Update(existingProfessor);
                await _context.SaveChangesAsync();

                return Ok(new ResultViewModel<Professor>(existingProfessor));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<string>("Falha ao atualizar o professor."));
            }
        }

        [HttpDelete("api/v1/professors/{id:int}")]
        [ProducesResponseType(typeof(ResultViewModel<string>), 200)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
        public async Task<IActionResult> Delete(int id)
        {
            var professor = await _context.Professors.FindAsync(id);

            if (professor == null)
                return NotFound(new ResultViewModel<string>("Professor n�o encontrado."));

            try
            {
                _context.Professors.Remove(professor);
                await _context.SaveChangesAsync();

                return Ok(new ResultViewModel<string>("Professor deletado com sucesso"));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<string>("Falha ao excluir o professor."));
            }
        }
    }
}