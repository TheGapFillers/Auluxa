using System.Data.Entity;
using Auluxa.WebApp.ApplicationContext;
using Auluxa.WebApp.Scenes.Models;

namespace Auluxa.WebApp.Scenes.Repositories
{
    public interface ISceneDbContext : IApplicationDbContext
    {
        DbSet<Scene> Scenes { get; set; }
        DbSet<Sequence> Sequences { get; set; }
        DbSet<Schedule> Schedules { get; set; }
        DbSet<Trigger> Triggers { get; set; }
    }
}
