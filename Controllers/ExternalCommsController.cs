using Microsoft.AspNetCore.Mvc;
using Jarvis_Brain.Services;
using Jarvis_Brain.Models;
using Microsoft.AspNetCore.Cors;

namespace Jarvis_Brain.Controllers
{
    public class ExternalCommsController : BaseController
    {
        private readonly IDHTService _dhtService;
        private readonly Jarvis_BrainDBContext _dbContext;
        public ExternalCommsController(IDHTService dHTService, Jarvis_BrainDBContext dbContext)
        {
            _dhtService = dHTService;
            _dbContext = dbContext;
        }

        public IActionResult GetLast7DaysTemperature(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return null;
            }
            else
            {
                return new JsonResult(this.ConvertFromEF(_dhtService.GetLast7DaysDHTPackage(Id)));
            }
        }

        public IActionResult GetLowestHighestTemperatureIn24Hours(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return null;
            }
            else
            {
                return new JsonResult(this.ConvertFromEF(_dhtService.GetLowestHighestTemperatureIn24Hours(Id)));
            }
        }

       public IActionResult AllTemperatures()
        {
            return new JsonResult(this.ConvertFromEF(_dhtService.AllTemperatures()));
        }
    }
}