using Jarvis_Brain.Models;

namespace Jarvis_Brain.Services
{
    public interface IDHTService
    {
        int SaveDHTRecord(DHTPackage dhtPackage);

        DHTPackage GetLatestDHTPackage();

        DHTCollection GetLast7DaysDHTPackage();

        DHTCollection GetLast7DaysDHTPackage(string locationName);

        DHTCollection GetLowestHighestTemperatureIn24Hours(string locationName);
    }
}
