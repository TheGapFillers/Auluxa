using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Auluxa.WebApp.Models;
using Auluxa.WebApp.Repositories.Contexts;

namespace Auluxa.WebApp.Repositories
{
	public class EfApplicationRepository : IApplicationRepository
	{
		public IApplicationDbContext Context { get; set; }

		#region Scene
		public async Task<IEnumerable<Scene>> GetScenesAsync(IEnumerable<int> ids = null)
		{
			List<int> idList = ids?.ToList();

			IQueryable<Scene> query = idList == null ?
				Context.Scenes :
				Context.Scenes.Where(s => idList.Contains(s.Id));

			IEnumerable<Scene> scenes = await query
				.Include(q => q.ApplianceSettings)
				.Include(q => q.ApplianceSettings.Select(s => s.Appliance))
				.Include(q => q.ApplianceSettings.Select(s => s.Appliance.Model))
				.Include(q => q.ApplianceSettings.Select(s => s.Appliance).Select(a => a.Zone))
				.Include(q => q.Sequence)
				.Include(q => q.Schedule)
				.ToListAsync();

			return scenes;
		}

		public async Task<Scene> CreateSceneAsync(Scene scene)
		{
			await BindSceneSettingsToAppliances(scene);

			if (scene.Schedule == null) scene.Schedule = new Schedule();
			if (scene.Sequence == null) scene.Sequence = new Sequence();
			Scene sceneToCreate = Context.Scenes.Add(scene);

			await SaveAsync();
			return sceneToCreate;
		}

		public async Task<Scene> UpdateSceneAsync(Scene scene)
		{
			Scene sceneToUpdate = (await GetScenesAsync(new List<int> { scene.Id })).SingleOrDefault();
			if (sceneToUpdate == null)
				return null;

			if (scene.ApplianceSettings != null)
			{
				await BindSceneSettingsToAppliances(scene);

				sceneToUpdate.ApplianceSettings = scene.ApplianceSettings;
			}
			if (scene.Name != null) sceneToUpdate.Name = scene.Name;
			if (scene.Schedule != null) sceneToUpdate.Schedule = scene.Schedule;
			if (scene.Sequence != null) sceneToUpdate.Sequence = scene.Sequence;

			await SaveAsync();
			return sceneToUpdate;
		}

		private async Task BindSceneSettingsToAppliances(Scene scene)
		{
			List<Appliance> usedAppliances =
				(await GetAppliancesAsync(scene.ApplianceSettings.Select(s => s.Appliance.Id))).ToList();

			foreach (ApplianceSetting setting in scene.ApplianceSettings)
			{
				setting.Appliance = usedAppliances.SingleOrDefault(a => a.Id == setting.Appliance.Id);
				if(!setting.IsValid())
				{
					throw new Exception($"Invalid setting for appliance {setting.Appliance.Id}");
				}
			}
		}

		public async Task<Scene> DeleteSceneAsync(int id)
		{
			Scene alreadyExistsScene = (await GetScenesAsync(new List<int> { id })).SingleOrDefault();
			if (alreadyExistsScene == null)
				return null;

			Scene deletedScene = Context.Scenes.Remove(alreadyExistsScene);
			await SaveAsync();
			return deletedScene;
		}

		#endregion
		#region Zone

		public async Task<IEnumerable<Zone>> GetZonesAsync(IEnumerable<int> ids = null)
		{
			List<int> idList = ids?.ToList();
			IQueryable<Zone> query = Context.Zones;

			if (idList != null)
				query = Context.Zones.Where(z => idList.Contains(z.Id));

			IEnumerable<Zone> zones = await query
				.Include(z => z.Appliances)
				.ToListAsync();

			return zones;
		}

		public async Task<Zone> CreateZoneAsync(Zone zone)
		{
			List<Appliance> usedAppliances =
				(await GetAppliancesAsync(zone.Appliances.Select(z => z.Id))).ToList();

			zone.Appliances = usedAppliances;

			Zone zoneToCreate = Context.Zones.Add(zone);

			await SaveAsync();
			return zoneToCreate;
		}

		public async Task<Zone> UpdateZoneAsync(Zone zone)
		{
			Zone zoneToUpdate = (await GetZonesAsync(new List<int> { zone.Id })).SingleOrDefault();
			if (zoneToUpdate == null)
				return null;

			if (zone.Appliances != null)
			{
				List<Appliance> usedAppliances =
				(await GetAppliancesAsync(zone.Appliances.Select(a => a.Id))).ToList();

				zoneToUpdate.Appliances = usedAppliances;
			}
			if (zone.Name != null) zoneToUpdate.Name = zone.Name;

			await SaveAsync();
			return zoneToUpdate;
		}

		public async Task<Zone> DeleteZoneAsync(int id)
		{
			Zone alreadExistsZone = (await GetZonesAsync(new List<int> { id })).SingleOrDefault();
			if (alreadExistsZone == null)
				return null;

			Zone deletedZone = Context.Zones.Remove(alreadExistsZone);
			await SaveAsync();
			return deletedZone;
		}

		public async Task<IEnumerable<Appliance>> GetAppliancesAsync(IEnumerable<int> ids = null)
		{
			List<int> idList = ids?.ToList();
			IQueryable<Appliance> query = Context.Appliances;

			if (idList != null)
				query = Context.Appliances.Where(z => idList.Contains(z.Id));

			IEnumerable<Appliance> appliances = await query
				.Include(a => a.Zone)
				.Include(a => a.Model)
				.ToListAsync();

			return appliances;
		}

		public async Task<Appliance> CreateApplianceAsync(Appliance appliance)
		{
			ApplianceModel usedModel = (await GetApplianceModelsAsync(new[] {appliance.Model.Id})).SingleOrDefault();
			if (usedModel == null)
				return null;

			appliance.Model = usedModel;

			if(appliance.CurrentSetting == null)
			{
				appliance.ApplyDefaultSettings();
			}
			else
			{
				if(!appliance.AreCurrentSettingsValid())
					throw new Exception("Invalid settings, must follow appliance model");
			}

			Appliance applianceToCreate = Context.Appliances.Add(appliance);

			await SaveAsync();
			return applianceToCreate;
		}

		public async Task<Appliance> UpdateApplianceAsync(Appliance appliance)
		{
			Appliance applianceToUpdate = (await GetAppliancesAsync(new List<int> { appliance.Id })).SingleOrDefault();
			if (applianceToUpdate == null)
				return null;

			if (appliance.Model != null)
			{
				ApplianceModel usedModel = (await GetApplianceModelsAsync(new[] { appliance.Model.Id })).SingleOrDefault();
				if (usedModel == null)
					return null;

				applianceToUpdate.Model = usedModel;
			}

			if (appliance.CurrentSetting != null)
			{
				if(!appliance.AreCurrentSettingsValid())
					throw new Exception("Invalid settings, must follow appliance model");

				applianceToUpdate.CurrentSetting = appliance.CurrentSetting;
			}

			if (appliance.Name != null) applianceToUpdate.Name = appliance.Name;
			if (appliance.Zone != null) applianceToUpdate.Zone = appliance.Zone;

			await SaveAsync();
			return applianceToUpdate;
		}

		public async Task<Appliance> DeleteApplianceAsync(int id)
		{
			Appliance alreadExistsAppliance = (await GetAppliancesAsync(new List<int> { id })).SingleOrDefault();
			if (alreadExistsAppliance == null)
				return null;

			Appliance deletedAppliance = Context.Appliances.Remove(alreadExistsAppliance);
			await SaveAsync();
			return deletedAppliance;
		}

		public async Task<IEnumerable<ApplianceModel>> GetApplianceModelsAsync(IEnumerable<int> ids = null)
		{
			List<int> idList = ids?.ToList();
			IQueryable<ApplianceModel> query = Context.ApplianceModels;

			if (idList != null)
				query = Context.ApplianceModels.Where(z => idList.Contains(z.Id));

			IEnumerable<ApplianceModel> applianceModels = await query.ToListAsync();

			return applianceModels;
		}

		public async Task<ApplianceModel> CreateApplianceModelAsync(ApplianceModel applianceModel)
		{
			ApplianceModel applianceModelToCreate = Context.ApplianceModels.Add(applianceModel);

			await SaveAsync();
			return applianceModelToCreate;
		}

		public async Task<ApplianceModel> UpdateApplianceModelAsync(ApplianceModel applianceModel)
		{
			ApplianceModel applianceModelToUpdate = (await GetApplianceModelsAsync(new List<int> { applianceModel.Id })).SingleOrDefault();
			if (applianceModelToUpdate == null)
				return null;

			if (applianceModel.BrandName != null) applianceModelToUpdate.BrandName = applianceModel.BrandName;
			if (applianceModel.Category != null) applianceModelToUpdate.Category = applianceModel.Category;
			if (applianceModel.ModelName != null) applianceModelToUpdate.ModelName = applianceModel.ModelName;
			if (applianceModel.PossibleSettings != null) applianceModelToUpdate.PossibleSettings = applianceModel.PossibleSettings;

			await SaveAsync();
			return applianceModelToUpdate;
		}

		public async Task<ApplianceModel> DeleteApplianceModelAsync(int id)
		{
			ApplianceModel existingApplianceModel = (await GetApplianceModelsAsync(new List<int> { id })).SingleOrDefault();
			if (existingApplianceModel == null)
				return null;

			ApplianceModel deletedApplianceModel = Context.ApplianceModels.Remove(existingApplianceModel);
			await SaveAsync();
			return deletedApplianceModel;
		}

		public async Task<Setting> CreateSettingAsync(Setting setting)
		{
			if (await GetSettingAsync() != null)
				return null;

			Setting settingToCreate = Context.Settings.Add(setting);

			await SaveAsync();
			return settingToCreate;
		}

		public async Task<Setting> GetSettingAsync()
		{
			return await Context.Settings.FirstAsync();
		}

		public async Task<Setting> UpdateSettingAsync(Setting setting)
		{
			Setting settingToUpdate = await GetSettingAsync();
			if (settingToUpdate == null)
				return await CreateSettingAsync(new Setting());

			if (setting.HoursFormat != null) settingToUpdate.HoursFormat = setting.HoursFormat;
			if (setting.DateFormat != null) settingToUpdate.DateFormat = setting.DateFormat;
			if (setting.TimeZoneName != null) settingToUpdate.TimeZoneName = setting.TimeZoneName;
			if (setting.TimeZoneUtcOffset != null) settingToUpdate.TimeZoneUtcOffset = setting.TimeZoneUtcOffset;

			await SaveAsync();
			return settingToUpdate;
		}

		public async Task<Kranium> CreateKraniumAsync(Kranium kranium)
		{
			if (await GetKraniumAsync() != null)
				return null;

			Kranium kraniumToCreate = Context.Kranium.Add(kranium);

			await SaveAsync();
			return kraniumToCreate;
		}

		public async Task<Kranium> GetKraniumAsync()
		{
			return await Context.Kranium.FirstAsync();
		}

		public async Task<Kranium> UpdateKraniumAsync(Kranium kranium)
		{
			Kranium kraniumToUpdate = await GetKraniumAsync();
			if (kraniumToUpdate == null)
				return await CreateKraniumAsync(new Kranium());

			if (kranium.Name != null) kraniumToUpdate.Name = kranium.Name;
			if (kranium.Version != null) kraniumToUpdate.Version = kranium.Version;
			if (kranium.MacAddress != null) kraniumToUpdate.MacAddress = kranium.MacAddress;
			if (kranium.IPAddress != null) kraniumToUpdate.IPAddress = kranium.IPAddress;
			if (kranium.ZigBeePanId != null) kraniumToUpdate.ZigBeePanId = kranium.ZigBeePanId;
			if (kranium.ZigBeeChannel != null) kraniumToUpdate.ZigBeeChannel = kranium.ZigBeeChannel;
			if (kranium.ZigBeeMacAddress != null) kraniumToUpdate.ZigBeeMacAddress = kranium.ZigBeeMacAddress;

			await SaveAsync();
			return kraniumToUpdate;
		}

		#endregion

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
