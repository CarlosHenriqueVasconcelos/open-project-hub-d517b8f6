using Microsoft.AspNetCore.Mvc;
using PlataformaGestaoIA.Attributes;

namespace PlataformaGestaoIA.Controllers
{
    [ApiController]
    
    public class HomeController : ControllerBase
    {
        [HttpGet("")]
        [ApiKey]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpGet("/v1")]
        [ApiKey]
        public IActionResult GetV1()
        {
            return Ok();
        }
    }

}
