using System.Data.Entity;
using Auluxa.WebApp.ApplicationContext;
using Auluxa.WebApp.Devices.Models;

namespace Auluxa.WebApp.Devices.Repositories
{
    public interface IDeviceDbContext : IApplicationDbContext
    {
        DbSet<DeviceModel> DeviceModels { get; set; }
        DbSet<Device> Devices { get; set; }
        DbSet<DeviceSetting> DeviceSettings { get; set; }
    }
}
