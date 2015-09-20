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

        public async Task<IEnumerable<Scene>> GetScenesAsync(IEnumerable<int> ids = null)
        {
            IQueryable<Scene> query = Context.Scenes.Where(s => ids.Contains(s.Id));

            IEnumerable<Scene> scenes = await query.ToListAsync();

            return scenes;
        }

        public async Task<Scene> UpsertSceneAsync(Scene scene)
        {
            Scene sceneToUpsert = (await GetScenesAsync(new List<int> { scene.Id })).SingleOrDefault();

            if (sceneToUpsert == null) // Insert
            {
                sceneToUpsert = Context.Scenes.Add(scene);
            }
            else // Update
            {         
                sceneToUpsert.Appliances = scene.Appliances;
                sceneToUpsert.Name = scene.Name;
                sceneToUpsert.Schedule = scene.Schedule;
                sceneToUpsert.Sequence = scene.Sequence;
            }

            await SaveAsync();
            return sceneToUpsert;
        }

        public async Task<Scene> DeleteSceneAsync(int id)
        {
            Scene alreadyExistScene = (await GetScenesAsync(new List<int> { id })).SingleOrDefault();

            return Context.Scenes.Remove(alreadyExistScene);
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
            {
                if (Context != null)
                    Context.Dispose();
            }
        }
    }
}
