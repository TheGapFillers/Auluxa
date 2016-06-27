using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Auluxa.WebApp.Devices.Models;

namespace Auluxa.WebApp.Devices.Repositories
{
    public class DeviceModelMap : EntityTypeConfiguration<DeviceModel>
    {
        public DeviceModelMap()
        {
            ToTable("DeviceModels", "Auluxa");
            HasKey(a => a.Id);
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);         
        }
    }
}
