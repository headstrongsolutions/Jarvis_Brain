namespace Jarvis_Brain.ViewModels
{
    using System;
    using Jarvis_Brain.Models;
    public class TemperatureReading
    {
        public string Humidity { get; set; }
        public string Temperature { get; set; }
        public string LocationName { get; set; }
        public DateTime Received { get; set; }

        public DhtPackages FromVMtoEF(TemperatureReading dhtPackage){
            return new DhtPackages(){
                Humidity = dhtPackage.Humidity,
                Temperature = dhtPackage.Temperature,
                Location = new Locations()
                {
                    LocationName = dhtPackage.LocationName
                },
                Received = dhtPackage.Received.ToString()
            };
        }

        public TemperatureReading FromEFtoVM(DhtPackages temperatureReading){
            return new TemperatureReading(){
                Humidity = temperatureReading.Humidity != null ? temperatureReading.Humidity : "none",
                Temperature = temperatureReading.Temperature != null ? temperatureReading.Temperature : "none",
                LocationName = temperatureReading.Location != null ? temperatureReading.Location.LocationName : string.Empty,
                Received = Convert.ToDateTime(temperatureReading.Received)
            };
        }
    }
}