using System.Data.Entity;
using Auluxa.WebApp.Appliances.Models;
using Auluxa.WebApp.ApplicationContext;

namespace Auluxa.WebApp.Appliances.Repositories
{
    public interface IApplianceDbContext : IApplicationDbContext
    {
        DbSet<ApplianceModel> ApplianceModels { get; set; }
        DbSet<Appliance> Appliances { get; set; }
        DbSet<ApplianceSetting> ApplianceSettings { get; set; }
    }
}
