using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Auluxa.Models;
using Auluxa.Repositories.Contexts;

namespace Auluxa.Repositories
{
	public class EfApplicationRepository : IApplicationRepository
	{
		public IApplicationDbContext Context { get; set; }

		#region Scene
		public async Task<IEnumerable<Scene>> GetScenesAsync(IEnumerable<int> ids = null)
		{
			List<int> idList = ids?.ToList();
			IQueryable<Scene> query;

			if (idList == null)
				query = Context.Scenes;
			else
				query = Context.Scenes.Where(s => idList.Contains(s.Id));

			IEnumerable<Scene> scenes = await query
				.Include(q => q.ApplianceSettings)
				.Include(q => q.ApplianceSettings.Select(s => s.Appliance))
				.Include(q => q.ApplianceSettings.Select(s => s.Appliance).Select(a => a.Zone))
				.Include(q => q.Sequence)
				.Include(q => q.Schedule)
				.ToListAsync();

			return scenes;
		}

		public async Task<Scene> UpsertSceneAsync(Scene scene)
		{
			Scene sceneToUpsert = (await GetScenesAsync(new List<int> { scene.Id })).SingleOrDefault();

			if (sceneToUpsert == null) // Insert
			{
				if (scene.Schedule == null) scene.Schedule = new Schedule();
				if (scene.Sequence == null) scene.Sequence = new Sequence();
				sceneToUpsert = Context.Scenes.Add(scene);
			}
			else // Update
			{
				sceneToUpsert.ApplianceSettings = scene.ApplianceSettings;
				sceneToUpsert.Name = scene.Name;
				sceneToUpsert.Schedule = scene.Schedule ?? new Schedule();
				sceneToUpsert.Sequence = scene.Sequence ?? new Sequence();
			}

			await SaveAsync();
			return sceneToUpsert;
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

		public async Task<Zone> UpsertZoneAsync(Zone zone)
		{
			Zone zoneToUpsert = (await GetZonesAsync(new List<int> { zone.Id })).SingleOrDefault();

			if (zoneToUpsert == null)   // Insert
			{
				if (zone.Appliances == null) zone.Appliances = new List<Appliance>();
				zoneToUpsert = Context.Zones.Add(zone);
			}
			else // Update
			{
				zoneToUpsert.Name = zone.Name;
				zoneToUpsert.Appliances = zone.Appliances ?? new List<Appliance>();
			}

			await SaveAsync();
			return zoneToUpsert;
		}

		public async Task<Zone> AttachAppliancesToZone(int zoneId, IEnumerable<int> applianceIds)
		{
			Zone zoneToUpdate  = (await GetZonesAsync(new List<int> { zoneId })).SingleOrDefault();
			if(zoneToUpdate == null)
				return null;

			foreach(var appliance in Context.Appliances.Where(a => applianceIds.Contains(a.ApplianceId)))
			{
				appliance.Zone = zoneToUpdate;
			}

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
			{
				if (Context != null)
					Context.Dispose();
			}
		}

	}
}
