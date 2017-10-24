using System;
using System.Web.Http;
using System.Net.Http.Formatting;
using Newtonsoft.Json;
using Jarvis_Brain.Code;
using Jarvis_Brain.Models;
using Jarvis_Brain.Services;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Jarvis_Brain.Controllers
{
    public class InternalCommsController : ApiController
    {
        ErrorFileLogger errorLog = new ErrorFileLogger("InternalCommsController");

        public InternalCommsController()
        {
        }

        [HttpPost]
        [Route("InternalComms/CollectDHT")]
        public IHttpActionResult CollectDHT([FromBody]DHTPackage dhtPackage)
        {
            try
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
                    saveResult = new DHTService().SaveDHTRecord(dhtPackage);
                }
                if (saveResult != 1)
                {
                    internalComms.State = StateEnum.Error;
                    internalComms.ErrorType = ErrorType.Unknown;
                }

                return new RawJsonActionResult(JsonConvert.SerializeObject(internalComms));
            }
            catch (Exception ex)
            {
                errorLog.WriteToErrorLog(ex.Message);
            }
            return null;
        }


        [HttpGet]
        [Route("InternalComms/")]
        public IHttpActionResult Get()
        {
            try
            {
                var internalComms = new InternalComms()
                {
                    Name = "Internal Comms",
                    Sent = DateTime.Now,
                    State = StateEnum.Sent,
                    Message = "This controller sends and receives all Internal Communications within the Jarvis Network."
                };

                return new RawJsonActionResult(JsonConvert.SerializeObject(internalComms));
            }
            catch (Exception ex)
            {
                errorLog.WriteToErrorLog(ex.Message);
            }
            return null;
        }

        [HttpGet]
        [Route("InternalComms/GetLatestTemperature")]
        public IHttpActionResult GetLatestTemperature()
        {
            try
            {
                DHTPackage dhtPackage = new DHTService().GetLatestDHTPackage();
                return new RawJsonActionResult(JsonConvert.SerializeObject(dhtPackage));
            }
            catch (Exception ex)
            {
                errorLog.WriteToErrorLog(ex.Message);
            }
            return null;
        }
    }
}
