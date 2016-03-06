using System.Data.Entity;
using Auluxa.WebApp.ApplicationContext;
using Auluxa.WebApp.Kranium.Models;

namespace Auluxa.WebApp.Kranium.Repositories
{
    public interface IKraniumDbContext : IApplicationDbContext
    {
        DbSet<KraniumEntity> Kranium { get; set; }
    }
}
