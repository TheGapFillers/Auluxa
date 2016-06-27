using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Auluxa.WebApp.Devices.Models;
using Auluxa.WebApp.Zones.Models;

namespace Auluxa.WebApp.Devices.Repositories
{
    public class EfDeviceRepository : IDeviceRepository
    {
        public IDeviceDbContext Context { get; set; }
        //public IZoneDbContext ZoneContext { get; set; }

        public async Task<IEnumerable<Device>> GetDevicesAsync(string userName)
        {
            IQueryable<Device> query = Context.Devices.Where(z => z.UserName == userName);

            IEnumerable<Device> devices = await query
                .Include(a => a.Zones)
                .Include(a => a.Model)
                .ToListAsync();

            return devices;
        }

        public async Task<IEnumerable<Device>> GetDevicesAsync(string userName, IEnumerable<int> ids)
        {
            IQueryable<Device> query = Context.Devices.Where(z => 
                z.UserName == userName && 
                ids.Contains(z.Id));

            IEnumerable<Device> devices = await query
                .Include(a => a.Zones)
                .Include(a => a.Model)
                .ToListAsync();

            return devices;
        }

        public async Task<Device> GetDeviceAsync(string userName, int id) =>
            (await GetDevicesAsync(userName, new[] {id})).FirstOrDefault();


        public async Task<Device> CreateDeviceAsync(string userName, int deviceModelid)
        {
            DeviceModel usedModel = await GetDeviceModelAsync(deviceModelid);
            if (usedModel == null)
                return null;

            var device = new Device { UserName = userName, Model = usedModel };
            if (device.CurrentSetting == null)
            {
                device.ApplyDefaultSettings();
            }
            else
            {
                if (!device.AreCurrentSettingsValid())
                    throw new Exception("Invalid settings, must follow device model");
            }

            Device deviceToCreate = Context.Devices.Add(device);

            await SaveAsync();
            return deviceToCreate;
        }

        public async Task<Device> UpdateDeviceZonesAsync(string userName, int deviceId, IEnumerable<Zone> zones)
        {
            Device device = await GetDeviceAsync(userName, deviceId);

            device.Zones = zones.ToList();

            await SaveAsync();
            return device;
        }

        public async Task<Device> UpdateDeviceSettingsAsync(string userName, int deviceId, Dictionary<string, string> deviceSettingsToUpdate)
        {
            Device device = await GetDeviceAsync(userName, deviceId);
            device.CurrentSetting = deviceSettingsToUpdate;

            await SaveAsync();
            return device;
        }

        public async Task<Device> DeleteDeviceAsync(string userName, int id)
        {
            Device alreadExistsDevice = (await GetDevicesAsync(userName, new [] { id })).SingleOrDefault();
            if (alreadExistsDevice == null)
                return null;

            Device deletedDevice = Context.Devices.Remove(alreadExistsDevice);
            await SaveAsync();
            return deletedDevice;
        }

        public async Task<IEnumerable<DeviceModel>> GetDeviceModelsAsync()
        {
            IQueryable<DeviceModel> query = Context.DeviceModels;

            IEnumerable<DeviceModel> deviceModels = await query.ToListAsync();

            return deviceModels;
        }

        public async Task<IEnumerable<DeviceModel>> GetDeviceModelsAsync(IEnumerable<int> ids)
        {
            IQueryable<DeviceModel> query = Context.DeviceModels.Where(z => ids.Contains(z.Id));

            IEnumerable<DeviceModel> deviceModels = await query.ToListAsync();

            return deviceModels;
        }

        public async Task<DeviceModel> GetDeviceModelAsync(int id) =>
            (await GetDeviceModelsAsync(new [] {id})).FirstOrDefault(); 


        public async Task<DeviceModel> CreateDeviceModelAsync(DeviceModel deviceModel)
        {
            DeviceModel deviceModelToCreate = Context.DeviceModels.Add(deviceModel);

            await SaveAsync();
            return deviceModelToCreate;
        }

        public async Task<DeviceModel> UpdateDeviceModelAsync(DeviceModel deviceModel)
        {
            DeviceModel deviceModelToUpdate = (await GetDeviceModelsAsync(new List<int> { deviceModel.Id })).SingleOrDefault();
            if (deviceModelToUpdate == null)
                return null;

            if (deviceModel.BrandName != null) deviceModelToUpdate.BrandName = deviceModel.BrandName;
            if (deviceModel.Category != null) deviceModelToUpdate.Category = deviceModel.Category;
            if (deviceModel.ModelName != null) deviceModelToUpdate.ModelName = deviceModel.ModelName;
            if (deviceModel.PossibleSettings != null) deviceModelToUpdate.PossibleSettings = deviceModel.PossibleSettings;

            await SaveAsync();
            return deviceModelToUpdate;
        }

        public async Task<DeviceModel> DeleteDeviceModelAsync(int id)
        {
            DeviceModel existingDeviceModel = await GetDeviceModelAsync(id);
            if (existingDeviceModel == null)
                return null;

            DeviceModel deletedDeviceModel = Context.DeviceModels.Remove(existingDeviceModel);
            await SaveAsync();
            return deletedDeviceModel;
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