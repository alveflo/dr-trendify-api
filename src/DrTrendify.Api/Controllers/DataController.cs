using Microsoft.AspNetCore.Mvc;
using DrTrendify.Core.Services.PopulateStockdetails;
using DrTrendify.Core.Services.GetStockdetails;
using DrTrendify.Core.Services.GetStockdetailById;
using System.Linq;
using DrTrendify.Core.FeatureToggling.Exceptions;

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

        [HttpGet("positive")]
        public IActionResult GetPositives()
        {
            var stockDetails = _getStockdetailsService.GetStockDetails()
                .Where(x => x.IsAllPositiveYield());

            return Ok(stockDetails);
        }

        [HttpGet("trending")]
        public IActionResult GetTrending()
        {
            var stockDetails = _getStockdetailsService.GetStockDetails()
                .Where(x => x.IsTrending() && x.DividendYield > 0);

            return Ok(stockDetails);
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_getStockdetailsService.GetStockDetails());
        }

        [HttpGet("populate")]
        public IActionResult PopulateData() 
        {
            try
            {
                _populateStockdetailsService.Populate();

                return Ok("Data was populated successfully.");
            }
            catch (FeatureFlagNotEnabledException e)
            {
                return Ok($"Data was not populated, due to kill switch is on: {e.FeatureFlagName}.");
            }
        }
    }
}