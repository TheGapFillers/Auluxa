using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Auluxa.WebApp.Scenes.Models;

namespace Auluxa.WebApp.Scenes.Repositories
{
    public class EfSceneRepository : ISceneRepository
    {
        public ISceneDbContext Context { get; set; }

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
                .Include(q => q.ApplianceSettings.Select(s => s.Appliance).Select(a => a.Zones))
                .Include(q => q.Sequence)
                .Include(q => q.Schedule)
                .ToListAsync();

            return scenes;
        }

        public async Task<Scene> CreateSceneAsync(Scene scene)
        {
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

           if (scene.Name != null) sceneToUpdate.Name = scene.Name;
            if (scene.Schedule != null) sceneToUpdate.Schedule = scene.Schedule;
            if (scene.Sequence != null) sceneToUpdate.Sequence = scene.Sequence;

            await SaveAsync();
            return sceneToUpdate;
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