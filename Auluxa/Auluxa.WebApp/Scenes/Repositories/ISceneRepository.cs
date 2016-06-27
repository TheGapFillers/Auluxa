using System.Collections.Generic;
using System.Threading.Tasks;
using Auluxa.WebApp.Scenes.Models;

namespace Auluxa.WebApp.Scenes.Repositories
{
    public interface ISceneRepository
    {
        Task<IEnumerable<Scene>> GetScenesAsync(string userName);
        Task<IEnumerable<Scene>> GetScenesAsync(string userName, IEnumerable<int> ids);
        Task<Scene> GetSceneAsync(string userName, int id);
        Task<Scene> CreateSceneAsync(Scene scene);
        Task<Scene> UpdateSceneAsync(Scene scene);
        Task<Scene> DeleteSceneAsync(string userName, int id);
    }
}
