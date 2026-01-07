using PlataformaGestaoIA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaGestaoIA.DataContext;
using PlataformaGestaoIA.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PlataformaGestaoIA.ViewModel;
using PlataformaGestaoIA.Attributes;
using Microsoft.AspNetCore.Authorization;

namespace PlataformaGestaoIA.Controllers
{
    [Authorize]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        [ProducesResponseType(typeof(ResultViewModel<Company>), 200)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
        [HttpPost("api/v1/companies")]
        public async Task<IActionResult> Post(
            [FromBody] Company company,
            [FromServices] PrincipalDataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

            try
            {
                await context.Companies.AddAsync(company);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Company>(company));
            }
            catch (DbUpdateException)
            {
                return StatusCode(400, new ResultViewModel<string>("05X99 - Esta empresa j� est� cadastrada"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna no servidor"));
            }
        }

        [ProducesResponseType(typeof(ResultViewModel<List<Company>>), 200)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
        [HttpGet("api/v1/companies/{id:int}")]
        public async Task<IActionResult> GetByIdAsync(
                [FromRoute] int id,
                [FromServices] PrincipalDataContext context)
        {
            try
            {
                var company = await context
                    .Companies
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (company == null)
                    return NotFound(new ResultViewModel<Company>("Empresa n�o encontrada"));

                return Ok(new ResultViewModel<Company>(company));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Company>("Falha interna no servidor"));
            }
        }

        [ProducesResponseType(typeof(ResultViewModel<Company>), 200)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
        [HttpGet("api/v1/companies")]
        public async Task<IActionResult> GetAllAsync(
                [FromServices] PrincipalDataContext context)
        {
            try
            {
                var companies = await context.Companies.ToListAsync();
                return Ok(new ResultViewModel<List<Company>>(companies));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Company>>("05X04 - Falha interna no servidor"));
            }
        }

        [ProducesResponseType(typeof(ResultViewModel<Company>), 200)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
        [HttpPut("api/v1/companies/{id:int}")]
        public async Task<IActionResult> PutAsync(
                [FromRoute] int id,
                [FromBody] Company company,
                [FromServices] PrincipalDataContext context)
        {
            try
            {
                var existingCompany = await context
                    .Companies
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (existingCompany == null)
                    return NotFound(new ResultViewModel<Company>("Empresa n�o encontrada"));

                existingCompany.Name = company.Name;
                existingCompany.LegalName = company.LegalName;
                existingCompany.CNPJ = company.CNPJ;

                context.Companies.Update(existingCompany);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Company>(existingCompany));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Company>("05XE8 - N�o foi poss�vel alterar a empresa"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<Company>("05X11 - Falha interna no servidor"));
            }
        }

        [ProducesResponseType(typeof(ResultViewModel<string>), 200)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 400)]
        [ProducesResponseType(typeof(ResultViewModel<string>), 500)]
        [HttpDelete("api/v1/companies/{id:int}")]
        public async Task<IActionResult> DeleteAsync(
                [FromRoute] int id,
                [FromServices] PrincipalDataContext context)
        {
            try
            {
                var company = await context
                    .Companies
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (company == null)
                    return NotFound(new ResultViewModel<Company>("Empresa n�o encontrada"));

                context.Companies.Remove(company);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<string>("Empresa deletada com sucesso"));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Company>("05XE7 - N�o foi poss�vel excluir a empresa"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<Company>("05X12 - Falha interna no servidor"));
            }
        }
    }
}