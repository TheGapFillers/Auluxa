﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Auluxa.WebApp.Appliances.Models;
using Auluxa.WebApp.Zones.Models;
using Auluxa.WebApp.Zones.Repositories;

namespace Auluxa.WebApp.Appliances.Repositories
{
	public class EfApplianceRepository : IApplianceRepository
	{
		public IApplianceDbContext Context { get; set; }
		public IZoneDbContext ZoneContext { get; set; }

		public async Task<IEnumerable<Appliance>> GetAppliancesAsync(IEnumerable<int> ids = null)
		{
			List<int> idList = ids?.ToList();
			IQueryable<Appliance> query = Context.Appliances;

			if (idList != null)
				query = Context.Appliances.Where(z => idList.Contains(z.Id));

			IEnumerable<Appliance> appliances = await query
				.Include(a => a.Zones)
				.Include(a => a.Model)
				.ToListAsync();

			return appliances;
		}

		public async Task<IEnumerable<Zone>> GetZonesAsync(IEnumerable<int> ids = null)
		{
			List<int> idList = ids?.ToList();
			IQueryable<Zone> query = ZoneContext.Zones;

			if (idList != null)
				query = ZoneContext.Zones.Where(z => idList.Contains(z.Id));

			IEnumerable<Zone> zones = await query
				.Include(z => z.Appliances)
				.ToListAsync();

			return zones;
		}

		public async Task<Appliance> CreateApplianceAsync(Appliance appliance)
		{
			ApplianceModel usedModel = (await GetApplianceModelsAsync(new[] { appliance.Model.Id })).SingleOrDefault();
			if (usedModel == null)
				return null;

			appliance.Model = usedModel;

			if (appliance.CurrentSetting == null)
			{
				appliance.ApplyDefaultSettings();
			}
			else
			{
				if (!appliance.AreCurrentSettingsValid())
					throw new Exception("Invalid settings, must follow appliance model");
			}

			Appliance applianceToCreate = Context.Appliances.Add(appliance);

			await SaveAsync();
			return applianceToCreate;
		}

		public async Task<Appliance> UpdateApplianceAsync(Appliance appliance)
		{
			throw new NotImplementedException();
			//Appliance applianceToUpdate = (await GetAppliancesAsync(new List<int> { appliance.Id })).SingleOrDefault();
			//if (applianceToUpdate == null)
			//	return null;

			//bool updatedModel = false;
			//if (appliance.Model != null)
			//{
			//	ApplianceModel usedModel = (await GetApplianceModelsAsync(new[] { appliance.Model.Id })).SingleOrDefault();
			//	if (usedModel == null)
			//		return null;

			//	appliance.Model = usedModel;
			//	applianceToUpdate.Model = usedModel;
			//	updatedModel = true;
			//}
			//else
			//{
			//	appliance.Model = (await GetApplianceModelsAsync(new[] { applianceToUpdate.Model.Id })).SingleOrDefault();
			//}

			//if (appliance.Zones != null)
			//{
			//	Zone usedZone = (await GetZonesAsync(new[] { appliance.Zones.Id })).SingleOrDefault();
			//	if (usedZone == null)
			//		return null;

			//	applianceToUpdate.Zones = usedZone;
			//}

			//if (appliance.CurrentSetting != null)
			//{
			//	if (!appliance.AreCurrentSettingsValid())
			//		throw new Exception("Invalid settings, must follow appliance model");

			//	applianceToUpdate.CurrentSetting = appliance.CurrentSetting;
			//}
			//else
			//{
			//	if(updatedModel)
			//	{
			//		applianceToUpdate.ApplyDefaultSettings();
			//	}
			//}
			
			//if (appliance.Name != null) applianceToUpdate.Name = appliance.Name;
			//if (appliance.UserName != null) applianceToUpdate.UserName = appliance.UserName;

			//await SaveAsync();
			//return applianceToUpdate;
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