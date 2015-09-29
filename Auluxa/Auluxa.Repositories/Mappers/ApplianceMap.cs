using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Auluxa.Models;

namespace Auluxa.Repositories.Mappers
{
    public class ApplianceMap : EntityTypeConfiguration<Appliance>
    {
        public ApplianceMap()
        {
            ToTable("Appliances", "Auluxa");
            HasKey(a => a.ApplianceId);
            Property(a => a.ApplianceId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasOptional(a => a.CurrentSetting);

            //HasRequired(a => a.Setting)
            //    .WithRequiredPrincipal(t => t.Appliance);

            HasOptional(a => a.Zone);
        }
    }
}
