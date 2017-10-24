using System;
using System.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json;
using Jarvis_Brain.Code;
using Jarvis_Brain.Models;
using Jarvis_Brain.Services;
using System.Collections.Generic;

namespace Jarvis_Brain.Controllers
{
    [EnableCors(origins: "http://boncester.serveftp.com", headers: "*", methods: "*")]
    public class ExternalCommsController : ApiController
    {
        ErrorFileLogger errorLog = new ErrorFileLogger("ExternalCommsController");

        public ExternalCommsController()
        {
        }

        [HttpGet]
        [Route("ExternalComms/GetLast7DaysTemperature")]
        public IHttpActionResult GetLast7DaysTemperature()
        {
            try
            {
                var dhtService = new DHTService();
                var locationName = string.Empty;
                DHTCollection dhtCollection = dhtService.GetLast7DaysDHTPackage(locationName);
                dhtCollection.ErrorLog = new ErrorLog(){
                    Location = "Get with no parameter"
                };
                Console.WriteLine(JsonConvert.SerializeObject(dhtCollection));
                return new RawJsonActionResult(JsonConvert.SerializeObject(dhtCollection));
            }
            catch (Exception ex)
            {
                errorLog.WriteToErrorLog(ex.Message);
            }
            return null;
        }

        [HttpGet]
        [Route("ExternalComms/GetLast7DaysTemperature/{locationName}")]
        public IHttpActionResult GetLast7DaysTemperature(string locationName)
        {
            try
            {
                DHTCollection dhtCollection = new DHTService().GetLast7DaysDHTPackage(locationName);
                return new RawJsonActionResult(JsonConvert.SerializeObject(dhtCollection));
            }
            catch (Exception ex)
            {
                errorLog.WriteToErrorLog(ex.Message);
            }
            return null;
        }

        [HttpGet]
        [Route("ExternalComms/GetLowestHighestTemperatureIn24Hours/{locationName}")]
        public IHttpActionResult GetLowestHighestTemperatureIn24Hours(string locationName)
        {
            try
            {
                var dhtService = new DHTService();
                DHTCollection minMaxTemps = dhtService.GetLowestHighestTemperatureIn24Hours(locationName);
                return new RawJsonActionResult(JsonConvert.SerializeObject(minMaxTemps));
            }
            catch (Exception ex)
            {
                errorLog.WriteToErrorLog(ex.Message);
            }
            return null;
        }
    }
}
