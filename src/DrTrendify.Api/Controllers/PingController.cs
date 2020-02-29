using System;
using Microsoft.AspNetCore.Mvc;

namespace DrTrendify.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public IActionResult Ping()
        {
            throw new NotImplementedException();
        }
    }
}