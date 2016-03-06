using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Auluxa.WebApp.UserSettings.Models;

namespace Auluxa.WebApp.UserSettings.Repositories
{
    public class EfUserSettingsRepository : IUserSettingsRepository
    {
        public IUserSettingsDbContext Context { get; set; }

        public async Task<UserSetting> CreateSettingAsync(UserSetting setting)
        {
            if (await GetSettingAsync() != null)
                return null;

            UserSetting settingToCreate = Context.Settings.Add(setting);

            await SaveAsync();
            return settingToCreate;
        }

        public async Task<UserSetting> GetSettingAsync()
        {
            return await Context.Settings.FirstAsync();
        }

        public async Task<UserSetting> UpdateSettingAsync(UserSetting setting)
        {
            UserSetting settingToUpdate = await GetSettingAsync();
            if (settingToUpdate == null)
                return await CreateSettingAsync(new UserSetting());

            if (setting.HoursFormat != null) settingToUpdate.HoursFormat = setting.HoursFormat;
            if (setting.DateFormat != null) settingToUpdate.DateFormat = setting.DateFormat;
            if (setting.TimeZoneName != null) settingToUpdate.TimeZoneName = setting.TimeZoneName;
            if (setting.TimeZoneUtcOffset != null) settingToUpdate.TimeZoneUtcOffset = setting.TimeZoneUtcOffset;

            await SaveAsync();
            return settingToUpdate;
        }

        public async Task<int> SaveAsync()
        {
            int count = await Context.SaveChangesAsync();
            return count;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                Context?.Dispose();
        }
    }
}