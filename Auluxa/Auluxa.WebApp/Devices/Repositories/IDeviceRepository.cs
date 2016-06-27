using System.Collections.Generic;
using System.Threading.Tasks;
using Auluxa.WebApp.Devices.Models;
using Auluxa.WebApp.Zones.Models;

namespace Auluxa.WebApp.Devices.Repositories
{
    public interface IDeviceRepository
    {
        Task<IEnumerable<Device>> GetDevicesAsync(string userName);
        Task<IEnumerable<Device>> GetDevicesAsync(string userName, IEnumerable<int> ids);
        Task<Device> GetDeviceAsync(string userName, int id);
        Task<Device> CreateDeviceAsync(string userName, int deviceId);
        Task<Device> UpdateDeviceSettingsAsync(string userName, int deviceId, Dictionary<string, string> deviceSettingsToUpdate);
        Task<Device> UpdateDeviceZonesAsync(string userName, int deviceId, IEnumerable<Zone> zones);
        Task<Device> DeleteDeviceAsync(string userName, int id);

        Task<IEnumerable<DeviceModel>> GetDeviceModelsAsync();
        Task<IEnumerable<DeviceModel>> GetDeviceModelsAsync(IEnumerable<int> ids);
        Task<DeviceModel> GetDeviceModelAsync(int id);
        Task<DeviceModel> CreateDeviceModelAsync(DeviceModel deviceModel);
        Task<DeviceModel> UpdateDeviceModelAsync(DeviceModel deviceModel);
        Task<DeviceModel> DeleteDeviceModelAsync(int id);
        
    }
}
