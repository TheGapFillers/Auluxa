using System.Collections.Generic;
using System.Threading.Tasks;
using Auluxa.WebApp.Zones.Models;

namespace Auluxa.WebApp.Zones.Repositories
{
    public interface IZoneRepository
    {
        Task<IEnumerable<Zone>> GetZonesAsync(string userName);
        Task<IEnumerable<Zone>> GetZonesAsync(string userName, IEnumerable<int> ids);
        Task<Zone> GetZoneAsync(string userName, int zoneId);
        Task<Zone> CreateZoneAsync(string userName, string name);
        Task<Zone> UpdateZoneAsync(string userName, int id, string name);
        Task<Zone> DeleteZoneAsync(string userName, int id);
    }
}
