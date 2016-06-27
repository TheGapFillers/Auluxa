using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Auluxa.WebApp.Devices.Models;

namespace Auluxa.WebApp.Devices.Repositories
{
    public class DeviceMap : EntityTypeConfiguration<Device>
    {
        public DeviceMap()
        {
            ToTable("Devices", "Auluxa");
            HasKey(a => a.Id);
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(a => a.Model);

            HasMany(a => a.Zones)
                .WithMany(z => z.Devices);
        }
    }
}
