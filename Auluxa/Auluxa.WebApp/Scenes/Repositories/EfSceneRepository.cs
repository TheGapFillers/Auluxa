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

        public async Task<IEnumerable<Scene>> GetScenesAsync(string userName)
        {
            IQueryable<Scene> query = Context.Scenes
                .Where(s => s.UserName == userName);

            IEnumerable<Scene> scenes = await query
                .Include(q => q.DeviceSettings)
                .Include(q => q.DeviceSettings.Select(s => s.Device))
                .Include(q => q.DeviceSettings.Select(s => s.Device.Model))
                .Include(q => q.DeviceSettings.Select(s => s.Device).Select(a => a.Zones))
                .Include(q => q.Sequence)
                .Include(q => q.Schedule)
                .ToListAsync();

            return scenes;
        }

        public async Task<IEnumerable<Scene>> GetScenesAsync(string userName, IEnumerable<int> ids)
        {
            IQueryable<Scene> query = Context.Scenes.Where(s => s.UserName == userName && ids.Contains(s.Id));

            IEnumerable<Scene> scenes = await query
                .Include(q => q.DeviceSettings)
                .Include(q => q.DeviceSettings.Select(s => s.Device))
                .Include(q => q.DeviceSettings.Select(s => s.Device.Model))
                .Include(q => q.DeviceSettings.Select(s => s.Device).Select(a => a.Zones))
                .Include(q => q.Sequence)
                .Include(q => q.Schedule)
                .ToListAsync();

            return scenes;
        }

        public async Task<Scene> GetSceneAsync(string userName, int id) =>
            (await GetScenesAsync(userName, new[] {id})).SingleOrDefault();

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
            Scene sceneToUpdate = await GetSceneAsync(scene.UserName, scene.Id);
            if (sceneToUpdate == null)
                return null;

            if (scene.Name != null) sceneToUpdate.Name = scene.Name;
            if (scene.DeviceSettings != null) sceneToUpdate.DeviceSettings = scene.DeviceSettings;
            if (scene.Schedule != null) sceneToUpdate.Schedule = scene.Schedule;
            if (scene.Sequence != null) sceneToUpdate.Sequence = scene.Sequence;

            await SaveAsync();
            return sceneToUpdate;
        }

        public async Task<Scene> DeleteSceneAsync(string userName, int id)
        {
            Scene alreadyExistsScene = await GetSceneAsync(userName, id);
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