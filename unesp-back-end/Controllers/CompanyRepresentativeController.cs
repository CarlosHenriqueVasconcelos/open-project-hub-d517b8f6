using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaGestaoIA.Controllers.Functions;
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
    [Route("api/v1/company-representatives")]
    public class CompanyRepresentativeController : ControllerBase
    {
        private readonly PrincipalDataContext _context;

        public CompanyRepresentativeController(PrincipalDataContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResultViewModel<CompanyRepresentative>), 200)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
        public async Task<IActionResult> PostAsync([FromBody] CompanyRepresentative model, [FromServices] PrincipalDataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

            await CompanyRepresentativeControllerFunction.TrackEntities(model, context);

            var representative = new CompanyRepresentative
            {
                Name = model.Name,
                InternalCode = model.InternalCode,
                CPF = model.CPF,
                Company = model.Company,
                User = model.User
            };

            try
            {
                await _context.CompanyRepresentatives.AddAsync(representative);
                await _context.SaveChangesAsync();

                return Ok(new ResultViewModel<CompanyRepresentative>(representative));
            }
            catch (DbUpdateException)
            {
                return StatusCode(400, new ResultViewModel<string>("05X99 - Este representante já está cadastrado"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna no servidor"));
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResultViewModel<CompanyRepresentative>), 200)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var representative = await _context
                    .CompanyRepresentatives
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (representative == null)
                    return NotFound(new ResultViewModel<CompanyRepresentative>("Representante não encontrado"));

                return Ok(new ResultViewModel<CompanyRepresentative>(representative));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("Falha interna no servidor"));
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResultViewModel<List<CompanyRepresentative>>), 200)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var representatives = await _context.CompanyRepresentatives.ToListAsync();
                return Ok(new ResultViewModel<List<CompanyRepresentative>>(representatives));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna no servidor"));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody]CompanyRepresentative model)
        {
            try
            {
                var representative = await _context
                    .CompanyRepresentatives
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (representative == null)
                    return NotFound(new ResultViewModel<CompanyRepresentative>("Conteúdo não encontrado"));

                representative.Name = model.Name;
                representative.InternalCode = model.InternalCode;
                representative.CPF = model.CPF;
                representative.Company = model.Company;
                representative.User = model.User;

                _context.CompanyRepresentatives.Update(representative);
                await _context.SaveChangesAsync();

                return Ok(new ResultViewModel<CompanyRepresentative>(representative));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<CompanyRepresentative>("05XE8 - Não foi possível alterar o representante"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<CompanyRepresentative>("05X11 - Falha interna no servidor"));
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResultViewModel<CompanyRepresentative>), 200)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var representative = await _context
                    .CompanyRepresentatives
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (representative == null)
                    return NotFound(new ResultViewModel<CompanyRepresentative>("Conteúdo não encontrado"));

                _context.CompanyRepresentatives.Remove(representative);
                await _context.SaveChangesAsync();

                return Ok(new ResultViewModel<string>("Representante excluído com sucesso"));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<CompanyRepresentative>("05XE7 - Não foi possível excluir o representante"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<CompanyRepresentative>("05X12 - Falha interna no servidor"));
            }
        }
    }
}