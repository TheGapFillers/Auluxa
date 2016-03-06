using System.Data.Entity;
using Auluxa.WebApp.ApplicationContext;
using Auluxa.WebApp.UserSettings.Models;

namespace Auluxa.WebApp.UserSettings.Repositories
{
    public interface IUserSettingsDbContext : IApplicationDbContext
    {
        DbSet<UserSetting> Settings { get; set; }
    }
}
