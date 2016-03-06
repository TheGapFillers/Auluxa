using System.Data.Entity;
using Auluxa.WebApp.ApplicationContext;
using Auluxa.WebApp.Zones.Models;

namespace Auluxa.WebApp.Zones.Repositories
{
    public interface IZoneDbContext : IApplicationDbContext
    {
        DbSet<Zone> Zones { get; set; }
    }
}
