using System.Collections.Generic;
using System.Threading.Tasks;
using Auluxa.WebApp.Zones.Models;

namespace Auluxa.WebApp.Zones.Repositories
{
    public interface IZoneRepository
    {
        Task<IEnumerable<Zone>> GetZonesAsync(IEnumerable<int> ids = null);
        Task<Zone> CreateZoneAsync(Zone zone);
        Task<Zone> UpdateZoneAsync(Zone zone);
        Task<Zone> DeleteZoneAsync(int id);
    }
}
