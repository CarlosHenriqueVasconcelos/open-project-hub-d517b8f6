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
    [Authorize]
    [ApiController]
    public class CourseSubjectController : ControllerBase
    {
        private readonly PrincipalDataContext _context;

        public CourseSubjectController(PrincipalDataContext context)
        {
            _context = context;
        }

        [HttpPost("api/v1/courseSubjects")]
        [ProducesResponseType(typeof(ResultViewModel<CourseSubject>), 200)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
        public async Task<IActionResult> Post([FromBody] CourseSubject courseSubject)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _context.CourseSubjects.AddAsync(courseSubject);
                await _context.SaveChangesAsync();

                return Ok(new ResultViewModel<CourseSubject>(courseSubject));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("Falha interna no servidor."));
            }
        }

        [HttpGet("api/v1/courseSubjects/{id:int}")]
        [ProducesResponseType(typeof(ResultViewModel<CourseSubject>), 200)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
        public async Task<IActionResult> GetById(int id)
        {
            var courseSubject = await _context.CourseSubjects.FindAsync(id);

            if (courseSubject == null)
                return NotFound(new ResultViewModel<CourseSubject>("Mat�ria do curso n�o encontrada."));

            return Ok(new ResultViewModel<CourseSubject>(courseSubject));
        }

        [ProducesResponseType(typeof(ResultViewModel<List<CourseSubject>>), 200)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
        [HttpGet("api/v1/courseSubjects")]
        public async Task<IActionResult> GetAll()
        {
            var courseSubjects = await _context.CourseSubjects.ToListAsync();
            return Ok(new ResultViewModel<List<CourseSubject>>(courseSubjects));
        }

        [ProducesResponseType(typeof(ResultViewModel<CourseSubject>), 200)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
        [HttpPut("api/v1/courseSubjects/{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] CourseSubject courseSubject)
        {
            if (id != courseSubject.Id)
                return BadRequest(new ResultViewModel<string>("Id da rota n�o corresponde ao Id da mat�ria do curso."));

            var existingCourseSubject = await _context.CourseSubjects.FindAsync(id);

            if (existingCourseSubject == null)
                return NotFound(new ResultViewModel<string>("Mat�ria do curso n�o encontrada."));

            existingCourseSubject.Name = courseSubject.Name;
            existingCourseSubject.Code = courseSubject.Code;

            try
            {
                _context.CourseSubjects.Update(existingCourseSubject);
                await _context.SaveChangesAsync();

                return Ok(new ResultViewModel<CourseSubject>(existingCourseSubject));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("Falha ao atualizar a mat�ria do curso."));
            }
        }

        [ProducesResponseType(typeof(ResultViewModel<string>), 200)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
        [HttpDelete("api/v1/courseSubjects/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var courseSubject = await _context.CourseSubjects.FindAsync(id);

            if (courseSubject == null)
                return NotFound(new ResultViewModel<string>("Mat�ria do curso n�o encontrada."));

            try
            {
                _context.CourseSubjects.Remove(courseSubject);
                await _context.SaveChangesAsync();

                return Ok(new ResultViewModel<string>("Curso exclu�do com sucesso"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("Falha ao excluir a mat�ria do curso."));
            }
        }
    }
}