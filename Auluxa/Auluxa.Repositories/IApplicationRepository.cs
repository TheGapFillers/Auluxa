using System.Collections.Generic;
using System.Threading.Tasks;
using Auluxa.Models;

namespace Auluxa.Repositories
{
	public interface IApplicationRepository
	{
		Task<IEnumerable<Scene>> GetScenesAsync(IEnumerable<int> ids = null);
		Task<Scene> UpsertSceneAsync(Scene scene);
		Task<Scene> DeleteSceneAsync(int id);

		Task<IEnumerable<Zone>> GetZonesAsync(IEnumerable<int> ids = null);
		Task<Zone> UpsertZoneAsync(Zone zone);
		Task<Zone> AttachAppliancesToZone(int zoneId, IEnumerable<int> applianceIds);
		Task<Zone> DeleteZoneAsync(int id);

		Task<IEnumerable<Appliance>> GetAppliancesAsync(IEnumerable<int> ids = null);
		Task<Appliance> UpsertApplianceAsync(Appliance appliance);
		Task<Appliance> DeleteApplianceAsync(int id);
	}
}
