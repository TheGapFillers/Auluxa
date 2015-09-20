using System.Collections.Generic;
using System.Threading.Tasks;
using Auluxa.Models;

namespace Auluxa.Repositories
{
    public interface IApplicationRepository
    {
        Task<IEnumerable<Scene>> GetScenesAsync(IEnumerable<int> ids = null);
        Task<Scene> UpsertSceneAsync(Scene scene);
        Task<Scene> DeleteSceneAsync(int id);
    }
}
