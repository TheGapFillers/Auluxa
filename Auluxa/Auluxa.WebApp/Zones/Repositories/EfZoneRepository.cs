using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Auluxa.WebApp.Zones.Models;

namespace Auluxa.WebApp.Zones.Repositories
{
    public class EfZoneRepository : IZoneRepository
    {
        public IZoneDbContext Context { get; set; }

        public async Task<IEnumerable<Zone>> GetZonesAsync(IEnumerable<int> ids = null)
        {
            List<int> idList = ids?.ToList();
            IQueryable<Zone> query = Context.Zones;

            if (idList != null)
                query = Context.Zones.Where(z => idList.Contains(z.Id));

            IEnumerable<Zone> zones = await query
                .Include(z => z.Appliances)
                .ToListAsync();

            return zones;
        }

        public async Task<Zone> CreateZoneAsync(Zone zone)
        {
            Zone zoneToCreate = Context.Zones.Add(zone);

            await SaveAsync();
            return zoneToCreate;
        }

        public async Task<Zone> UpdateZoneAsync(Zone zone)
        {
            Zone zoneToUpdate = (await GetZonesAsync(new List<int> { zone.Id })).SingleOrDefault();
            if (zoneToUpdate == null)
                return null;

            if (zone.Name != null) zoneToUpdate.Name = zone.Name;

            await SaveAsync();
            return zoneToUpdate;
        }

        public async Task<Zone> DeleteZoneAsync(int id)
        {
            Zone alreadExistsZone = (await GetZonesAsync(new List<int> { id })).SingleOrDefault();
            if (alreadExistsZone == null)
                return null;

            Zone deletedZone = Context.Zones.Remove(alreadExistsZone);
            await SaveAsync();
            return deletedZone;
        }

        public async Task<int> SaveAsync()
        {
            int count = await Context.SaveChangesAsync();
            return count;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                Context?.Dispose();
        }
    }
}