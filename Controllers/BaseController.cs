using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Jarvis_Brain.Code;
using Jarvis_Brain.Models;
using Jarvis_Brain.Services;
using Microsoft.AspNetCore.Mvc;
using Jarvis_Brain.ViewModels;
using System.Linq;

namespace Jarvis_Brain.Controllers
{
    /// <summary>
    /// Adds in base layer functionality to the controller base
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// Converts from EF models to return Temperature viewmodels
        /// </summary>
        /// <param name="dhtPackages"></param>
        /// <returns></returns>
         public List<TemperatureReading> ConvertFromEF (IQueryable<DhtPackages> dhtPackages){
            var temperatures = new List<TemperatureReading>();
            foreach(var efRecord in dhtPackages){
                temperatures.Add(new TemperatureReading().FromEFtoVM(efRecord));
            }
            return temperatures;
        }
    }
}