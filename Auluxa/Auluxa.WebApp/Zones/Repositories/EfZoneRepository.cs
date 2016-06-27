using System;
using System.Collections;
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

        public async Task<IEnumerable<Zone>> GetZonesAsync(string userName)
        {
            IQueryable<Zone> query = Context.Zones.Where(z => z.UserName == userName);

            IEnumerable<Zone> zones = await query
                .Include(z => z.Devices)
                .ToListAsync();

            return zones;
        }

        public async Task<IEnumerable<Zone>> GetZonesAsync(string userName, IEnumerable<int> ids)
        {
            IQueryable<Zone> query = Context.Zones.Where(z => 
                z.UserName == userName &&
                ids.Contains(z.Id)); 

            IEnumerable<Zone> zones = await query
                .Include(z => z.Devices)
                .ToListAsync();

            return zones;
        }

        public async Task<Zone> GetZoneAsync(string userName, int zoneId) =>
            (await GetZonesAsync(userName, new[] {zoneId})).FirstOrDefault();



        public async Task<Zone> CreateZoneAsync(string userName, string name)
        {
            Zone zoneToCreate = Context.Zones.Add(new Zone {UserName = userName, Name = name} );

            await SaveAsync();
            return zoneToCreate;
        }

        public async Task<Zone> UpdateZoneAsync(string userName, int id, string name)
        {
            Zone zoneToUpdate = await GetZoneAsync(userName, id);
            if (zoneToUpdate == null)
                return null;

            zoneToUpdate.Name = name;

            await SaveAsync();
            return zoneToUpdate;
        }

        public async Task<Zone> DeleteZoneAsync(string userName, int id)
        {
            Zone alreadExistsZone = await GetZoneAsync(userName, id);
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