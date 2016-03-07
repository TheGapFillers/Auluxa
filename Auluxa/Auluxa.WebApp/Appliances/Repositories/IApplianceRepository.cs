using System.Collections.Generic;
using System.Threading.Tasks;
using Auluxa.WebApp.Appliances.Models;

namespace Auluxa.WebApp.Appliances.Repositories
{
    public interface IApplianceRepository
    {
        Task<IEnumerable<Appliance>> GetAppliancesAsync(IEnumerable<int> ids = null);
        Task<Appliance> CreateApplianceAsync(Appliance appliance);
        Task<Appliance> UpdateApplianceAsync(Appliance appliance);
        Task<Appliance> DeleteApplianceAsync(int id);

        Task<IEnumerable<ApplianceModel>> GetApplianceModelsAsync(IEnumerable<int> ids = null);
        Task<ApplianceModel> CreateApplianceModelAsync(ApplianceModel applianceModel);
        Task<ApplianceModel> UpdateApplianceModelAsync(ApplianceModel applianceModel);
        Task<ApplianceModel> DeleteApplianceModelAsync(int id);
    }
}
