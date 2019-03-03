using Jarvis_Brain.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using MoreLinq;
using Microsoft.EntityFrameworkCore;

namespace Jarvis_Brain.Services
{
    /// <summary>
    /// DHT Service to manage DHT records between database and controllers
    /// </summary>
    public class DHTService : IDHTService
    {
        /// <summary>
        /// The database context
        /// </summary>
        private readonly Jarvis_BrainDBContext _dbContext;

        /// <summary>
        /// DHT Service constructor
        /// </summary>
        /// <param name="dbcontext">returns the Database Context</param>
        public DHTService(Jarvis_BrainDBContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        public IQueryable<DhtPackages> AllTemperatures()
        {
            return _dbContext.DhtPackages.Include(d => d.Location).Where(x => x.DhtPackageId != null);
        }

        /// <summary>
        /// Gets the latest recorded temperature by location name
        /// </summary>
        /// <param name="locationName">location name</param>
        /// <returns>Most recently recorded temperature package</returns>
        public IQueryable<DhtPackages> GetLatestDHTPackage(string locationName)
        {
            return _dbContext.DhtPackages.Include(d => d.Location).Where(w => w.Location.LocationName == locationName).OrderByDescending(r => r.Received).Take(1);
        }

        /// <summary>
        /// Gets the lowest and highest temperatures in the last 24 hours
        /// recorded within the location name
        /// </summary>
        /// <param name="locationName">location name</param>
        /// <returns>Lowest and highest recorded temperature packages</returns>
        public IQueryable<DhtPackages> GetLowestHighestTemperatureIn24Hours(string locationName)
        {
            var lowestHighestTemperatures = new List<DhtPackages>();
            var allIn24Hours = _dbContext.DhtPackages.Include(d => d.Location).Where(x => Convert.ToDateTime(x.Received) > DateTime.Now.AddDays(-1) && x.Location.LocationName.ToLower() == locationName.ToLower());

            lowestHighestTemperatures.Add(allIn24Hours.MaxBy(x => x.Temperature).FirstOrDefault());
            lowestHighestTemperatures.Add(allIn24Hours.MinBy(x => x.Temperature).FirstOrDefault());

            return lowestHighestTemperatures.AsQueryable();
        }

        /// <summary>
        /// The last 7 days worth or temperature package records
        /// from a specific location
        /// </summary>
        /// <param name="locationName">location name</param>
        /// <returns>7 days worth of temperature package records</returns>
        public IQueryable<DhtPackages> GetLast7DaysDHTPackage(string locationName)
        {
            var temperatures = _dbContext
                .DhtPackages
                .Include(y => y.Location)
                .Where(x => Convert.ToDateTime(x.Received) > DateTime.Now.AddDays(-7) && String.IsNullOrEmpty(locationName) || x.Location.LocationName.ToLower() == locationName.ToLower())
                ;
            return temperatures;
        }

        /// <summary>
        /// Saves a single DHT temperature record package
        /// </summary>
        /// <param name="dhtPackage">DHT Package</param>
        /// <returns>Int signifying EF success</returns>
        public int SaveDHTRecord(DhtPackages dhtPackage)
        {
            _dbContext.DhtPackages.Add(dhtPackage);
            return _dbContext.SaveChanges();
        }
    }
}