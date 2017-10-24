using Jarvis_Brain.Models;

namespace Jarvis_Brain.Services
{
    public interface IDHTService
    {
        int SaveDHTRecord(DHTPackage dhtPackage);

        DHTPackage GetLatestDHTPackage();
    }
}
