using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Auluxa.WebApp.Appliances.Models;

namespace Auluxa.WebApp.Appliances.Repositories
{
    public class ApplianceMap : EntityTypeConfiguration<Appliance>
    {
        public ApplianceMap()
        {
            ToTable("Appliances", "Auluxa");
            HasKey(a => a.Id);
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(a => a.Model);
            HasOptional(a => a.Zone);
        }
    }
}
