using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Feex.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Test()
        {

            return Ok();
        }
    }
}
