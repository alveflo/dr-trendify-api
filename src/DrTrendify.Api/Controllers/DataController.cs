using Microsoft.AspNetCore.Mvc;
using DrTrendify.Core.Services.PopulateStockdetails;
using DrTrendify.Core.Services.GetStockdetails;

namespace DrTrendify.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {
        private readonly IPopulateStockdetailsService _populateStockdetailsService;
        private readonly IGetStockdetailsService _getStockdetailsService;

        public DataController(
            IPopulateStockdetailsService populateStockdetailsService,
            IGetStockdetailsService getStockdetailsService)
        {
            _populateStockdetailsService = populateStockdetailsService;
            _getStockdetailsService = getStockdetailsService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_getStockdetailsService.GetStockDetails());
        }

        [HttpGet("populate")]
        public IActionResult PopulateData() 
        {
            _populateStockdetailsService.Populate();

            return Ok();
        }
    }
}