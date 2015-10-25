using System.Data.Entity.ModelConfiguration;
using Auluxa.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auluxa.Repositories.Mappers
{
    public class ZoneMap : EntityTypeConfiguration<Zone>
    {
        public ZoneMap()
        {
            ToTable("Zones", "Auluxa");
            HasKey(z => z.Id);
            Property(s => s.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasMany(z => z.Appliances).WithOptional(a => a.Zone);
        }
    }
}
