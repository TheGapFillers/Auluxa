using System.Collections.Generic;
using System.Threading.Tasks;
using Auluxa.WebApp.Models;

namespace Auluxa.WebApp.Repositories
{
	public interface IApplicationRepository
	{
		Task<IEnumerable<Scene>> GetScenesAsync(IEnumerable<int> ids = null);
		Task<Scene> CreateSceneAsync(Scene scene);
		Task<Scene> UpdateSceneAsync(Scene scene);
		Task<Scene> DeleteSceneAsync(int id);

		Task<IEnumerable<Zone>> GetZonesAsync(IEnumerable<int> ids = null);
		Task<Zone> CreateZoneAsync(Zone zone);
		Task<Zone> UpdateZoneAsync(Zone zone);
		Task<Zone> DeleteZoneAsync(int id);

		Task<IEnumerable<Appliance>> GetAppliancesAsync(IEnumerable<int> ids = null);
		Task<Appliance> CreateApplianceAsync(Appliance appliance);
		Task<Appliance> UpdateApplianceAsync(Appliance appliance);
		Task<Appliance> DeleteApplianceAsync(int id);

		Task<IEnumerable<ApplianceModel>> GetApplianceModelsAsync(IEnumerable<int> ids = null);
		Task<ApplianceModel> CreateApplianceModelAsync(ApplianceModel applianceModel);
		Task<ApplianceModel> UpdateApplianceModelAsync(ApplianceModel applianceModel);
		Task<ApplianceModel> DeleteApplianceModelAsync(int id);

		Task<Setting> CreateSettingAsync(Setting setting);
		Task<Setting> GetSettingAsync();
		Task<Setting> UpdateSettingAsync(Setting setting);

		Task<Kranium> CreateKraniumAsync(Kranium kranium);
		Task<Kranium> GetKraniumAsync();
		Task<Kranium> UpdateKraniumAsync(Kranium kranium);
	}
}
