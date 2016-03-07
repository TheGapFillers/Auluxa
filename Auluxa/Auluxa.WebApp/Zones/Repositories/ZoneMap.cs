using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Auluxa.WebApp.Zones.Models;

namespace Auluxa.WebApp.Zones.Repositories
{
    public class ZoneMap : EntityTypeConfiguration<Zone>
    {
        public ZoneMap()
        {
            ToTable("Zones", "Auluxa");
            HasKey(z => z.Id);
            Property(z => z.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasMany(z => z.Appliances).WithOptional(a => a.Zone);
        }
    }
}
