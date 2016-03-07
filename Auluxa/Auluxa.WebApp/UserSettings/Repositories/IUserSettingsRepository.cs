using System.Threading.Tasks;
using Auluxa.WebApp.UserSettings.Models;

namespace Auluxa.WebApp.UserSettings.Repositories
{
    public interface IUserSettingsRepository
    {
        Task<UserSetting> CreateSettingAsync(UserSetting setting);
        Task<UserSetting> GetSettingAsync();
        Task<UserSetting> UpdateSettingAsync(UserSetting setting);
    }
}
