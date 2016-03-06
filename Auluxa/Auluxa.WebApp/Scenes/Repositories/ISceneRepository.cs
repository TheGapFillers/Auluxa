using System.Collections.Generic;
using System.Threading.Tasks;
using Auluxa.WebApp.Scenes.Models;

namespace Auluxa.WebApp.Scenes.Repositories
{
    public interface ISceneRepository
    {
        Task<IEnumerable<Scene>> GetScenesAsync(IEnumerable<int> ids = null);
        Task<Scene> CreateSceneAsync(Scene scene);
        Task<Scene> UpdateSceneAsync(Scene scene);
        Task<Scene> DeleteSceneAsync(int id);
    }
}
