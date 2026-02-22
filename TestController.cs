using Microsoft.AspNetCore.Mvc;

namespace Cars24API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("API is working");
        }
    }
}
