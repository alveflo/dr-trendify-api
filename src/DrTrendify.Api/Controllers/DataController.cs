using Microsoft.AspNetCore.Mvc;
using DrTrendify.Core.Services.PopulateStockdetails;
using DrTrendify.Core.Services.GetStockdetails;
using DrTrendify.Core.Services.GetStockdetailById;
using System.Linq;
using DrTrendify.Core.FeatureToggling.Exceptions;
using DrTrendify.Core.Entities;

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

        [HttpGet("trending/{market}")]
        public IActionResult GetTrendingByMarket(Market market)
        {
            var stockDetails = _getStockdetailsService.GetStockDetails()
                .Where(x => x.IsTrending() && x.DividendYield > 0)
                .Where(x => x.Market == market);

            return Ok(stockDetails);
        }

        [HttpGet("trending")]
        public IActionResult GetTrending()
        {
            var stockDetails = _getStockdetailsService.GetStockDetails()
                .Where(x => x.IsTrending() && x.DividendYield > 0)
                .OrderByDescending(x => x.YieldOneMonth)
                .ThenByDescending(x => x.YieldThreeMonths)
                .ThenByDescending(x => x.YieldSixMonths)
                .ThenByDescending(x => x.YieldOneYear)
                .ToList();

            return Ok(stockDetails);
        }

        [HttpGet("babyrage")]
        public IActionResult GetTrendingBabyRage()
        {
            var stockDetails = _getStockdetailsService.GetStockDetails()
                .Where(x => x.IsBabyrageTrending)
                .OrderByDescending(x => x.YieldOneMonth)
                .ThenByDescending(x => x.YieldThreeMonths)
                .ThenByDescending(x => x.YieldSixMonths)
                .ThenByDescending(x => x.YieldOneYear)
                .ToList();

            return Ok(stockDetails);
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_getStockdetailsService.GetStockDetails());
        }

        [HttpGet("populate/{market}")]
        public IActionResult PopulateData(Market market) 
        {
            try
            {
               _populateStockdetailsService.Populate(market);

                return Ok($"Data was successfully populated for {market}.");
            }
            catch (FeatureFlagNotEnabledException e)
            {
                return Ok($"Data was not populated, due to kill switch is on: {e.FeatureFlagName}.");
            }
        }
    }
}