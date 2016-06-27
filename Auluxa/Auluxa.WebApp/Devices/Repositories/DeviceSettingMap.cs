using System.Data.Entity.ModelConfiguration;
using Auluxa.WebApp.Devices.Models;

namespace Auluxa.WebApp.Devices.Repositories
{
    public class DeviceSettingMap : EntityTypeConfiguration<DeviceSetting>
    {
        public DeviceSettingMap()
        {
            ToTable("DeviceSettings", "Auluxa");
            HasKey(a => a.Id);

            HasRequired(s => s.Device);
        }
    }
}
