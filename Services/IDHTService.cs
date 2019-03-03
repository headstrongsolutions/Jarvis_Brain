using Jarvis_Brain.Models;
using System.Collections.Generic;
using System.Linq;

namespace Jarvis_Brain.Services
{
    public interface IDHTService
    {
        int SaveDHTRecord(DhtPackages dhtPackage);

        IQueryable<DhtPackages> GetLatestDHTPackage(string locationName);

        IQueryable<DhtPackages> GetLast7DaysDHTPackage(string locationName);

        IQueryable<DhtPackages> GetLowestHighestTemperatureIn24Hours(string locationName);

        IQueryable<DhtPackages> AllTemperatures();
    }
}