using Microsoft.AspNetCore.Mvc;
using DrTrendify.Core.Services.PopulateStockdetails;
using DrTrendify.Core.Services.GetStockdetails;
using DrTrendify.Core.Services.GetStockdetailById;

namespace DrTrendify.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {
        private readonly IPopulateStockdetailsService _populateStockdetailsService;
        private readonly IGetStockdetailByIdService _getStockdetailByIdService;
        private readonly IGetStockdetailsService _getStockdetailsService;

        public DataController(
            IPopulateStockdetailsService populateStockdetailsService,
            IGetStockdetailByIdService getStockdetailByIdService,
            IGetStockdetailsService getStockdetailsService)
        {
            _populateStockdetailsService = populateStockdetailsService;
            _getStockdetailByIdService = getStockdetailByIdService;
            _getStockdetailsService = getStockdetailsService;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var result = _getStockdetailByIdService.GetById(id);
            if (result != null)
                return Ok(result);

            return NotFound();
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