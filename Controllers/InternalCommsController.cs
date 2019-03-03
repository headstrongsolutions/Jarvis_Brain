using System;
using Jarvis_Brain.Code;
using Jarvis_Brain.Models;
using Jarvis_Brain.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Jarvis_Brain.Controllers
{
    [EnableCors("GerdenTemperatureSensorHost")]
    public class InternalCommsController : BaseController
    {
        private readonly IDHTService _dhtService;
        private readonly Jarvis_BrainDBContext _dbContext;
        public InternalCommsController(IDHTService dHTService, Jarvis_BrainDBContext dbContext)
        {
            _dhtService = dHTService;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Saves a Temperature Reading to the Database
        /// </summary>
        /// <param name="dhtPackage">DHT Package</param>
        /// <returns>Internal Comms package indicating success of the Saved Reading</returns>
        public IActionResult CollectDHT(DhtPackages dhtPackage)
        {
            var internalComms = new InternalComms()
            {
                Name = "Collect DHT Package",
                Message = "Saving posted DHT data to the database",
                State = StateEnum.Received,
                Sent = DateTime.Now
            };

            var saveResult = 0;
            if (ModelState.IsValid)
            {
                saveResult = _dhtService.SaveDHTRecord(dhtPackage);
            }
            if (saveResult != 1)
            {
                internalComms.State = StateEnum.Error;
                internalComms.ErrorType = ErrorType.Unknown;
            }

            return new JsonResult(internalComms);
        }

        /// <summary>
        /// Gets the latest stored temperature from the Database against a Location Name
        /// </summary>
        /// <param name="locationName">A Locations Name</param>
        /// <returns>A collection with one entry of Temperature Readings</returns>
        public IActionResult GetLatestTemperature(string locationName)
        {
            return new JsonResult(this.ConvertFromEF(_dhtService.GetLatestDHTPackage(locationName)));
        }
    }
}