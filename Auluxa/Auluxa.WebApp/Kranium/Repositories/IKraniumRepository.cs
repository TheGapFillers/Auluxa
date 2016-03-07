using System.Threading.Tasks;
using Auluxa.WebApp.Kranium.Models;

namespace Auluxa.WebApp.Kranium.Repositories
{
    public interface IKraniumRepository
    {
        Task<KraniumEntity> CreateKraniumAsync(KraniumEntity kranium);
        Task<KraniumEntity> GetKraniumAsync();
        Task<KraniumEntity> UpdateKraniumAsync(KraniumEntity kranium);
    }
}
