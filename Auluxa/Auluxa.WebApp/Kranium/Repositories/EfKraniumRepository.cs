using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Auluxa.WebApp.Kranium.Models;

namespace Auluxa.WebApp.Kranium.Repositories
{
    public class EfKraniumRepository : IKraniumRepository
    {
        public IKraniumDbContext Context { get; set; }

        public async Task<KraniumEntity> CreateKraniumAsync(KraniumEntity kranium)
        {
            if (await GetKraniumAsync() != null)
                return null;

            KraniumEntity kraniumToCreate = Context.Kranium.Add(kranium);

            await SaveAsync();
            return kraniumToCreate;
        }

        public async Task<KraniumEntity> GetKraniumAsync()
        {
            return await Context.Kranium.FirstAsync();
        }

        public async Task<KraniumEntity> UpdateKraniumAsync(KraniumEntity kranium)
        {
            KraniumEntity kraniumToUpdate = await GetKraniumAsync();
            if (kraniumToUpdate == null)
                return await CreateKraniumAsync(new KraniumEntity());

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