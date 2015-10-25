using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Auluxa.Models;

namespace Auluxa.Repositories.Contexts
{
    public interface IApplicationDbContext : IDisposable
    {
        DbSet<Scene> Scenes { get; set; }
        DbSet<Sequence> Sequences { get; set; }
        DbSet<Schedule> Schedules { get; set; }
        DbSet<ApplianceModel> ApplianceModels { get; set; }
        DbSet<Appliance> Appliances { get; set; }
        DbSet<ApplianceSetting> ApplianceSettings { get; set; }
        DbSet<Zone> Zones { get; set; }
        DbSet<Trigger> Triggers { get; set; }

        Task<int> SaveChangesAsync();
    }
}
