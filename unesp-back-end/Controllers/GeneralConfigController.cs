using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaGestaoIA.DataContext;
using PlataformaGestaoIA.Extensions;
using PlataformaGestaoIA.Models;
using PlataformaGestaoIA.ViewModel;
using PlataformaGestaoIA.Controllers.Functions;

namespace PlataformaGestaoIA.Controllers
{
    [ApiController]
    public class GeneralConfigController : ControllerBase
    {
        private readonly PrincipalDataContext _context;

        public GeneralConfigController(PrincipalDataContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost("api/v1/general-configs")]
        public async Task<IActionResult> Post([FromBody] EditorGeneralConfigViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var config = await _context.GeneralConfigs.FirstOrDefaultAsync();
            
            try
            {
                if (config == null)
                {
                    config = new GeneralConfig
                    {
                        ConfigHeader = model.ConfigHeader,
                        ConfigBody = model.ConfigBody,
                        ConfigEmailDomainAvaliable = model.ConfigEmailDomainAvaliable,
                        ConfigConsent = model.ConfigConsent
                    };
                    
                    await _context.GeneralConfigs.AddAsync(config);
                    await _context.SaveChangesAsync();

                    return Ok(new ResultViewModel<GeneralConfig>(config));
                }
                else
                {
                    config.ConfigHeader = model.ConfigHeader;
                    config.ConfigBody = model.ConfigBody;
                    config.ConfigEmailDomainAvaliable = model.ConfigEmailDomainAvaliable;
                    config.ConfigConsent = model.ConfigConsent;

                    _context.GeneralConfigs.Update(config);
                    await _context.SaveChangesAsync();
                    return Ok(new ResultViewModel<GeneralConfig>(config));
                }               
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<string>("Falha interna no servidor."));
            }
        }

        [HttpGet("api/v1/general-configs")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var config = await _context.GeneralConfigs.FirstOrDefaultAsync();

                if (config == null)
                    return NotFound(new ResultViewModel<GeneralConfig>("Configuração não encontrada."));

                return Ok(new ResultViewModel<GeneralConfig>(config));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<string>("Falha interna no servidor."));
            }
        }

        [Authorize]
        [HttpDelete("api/v1/general-configs")]
        public async Task<IActionResult> Delete()
        {
            var config = await _context.GeneralConfigs.FirstOrDefaultAsync();
            if (config == null)
                return NotFound(new ResultViewModel<string>("Configuração não encontrada."));

            try
            {
                _context.GeneralConfigs.Remove(config);
                await _context.SaveChangesAsync();
                return Ok(new ResultViewModel<string>("Configuração excluída com sucesso"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<string>("Falha ao excluir a configuração."));
            }
        }
    }
}