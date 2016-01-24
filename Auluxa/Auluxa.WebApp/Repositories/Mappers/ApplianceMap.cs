using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Auluxa.WebApp.Models;

namespace Auluxa.WebApp.Repositories.Mappers
{
    public class ApplianceMap : EntityTypeConfiguration<Appliance>
    {
        public ApplianceMap()
        {
            ToTable("Appliances", "Auluxa");
            HasKey(a => a.Id);
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(a => a.Model);
			HasOptional(a => a.CurrentSetting).WithRequired(s => s.Appliance);
			HasOptional(a => a.Zone);
		}
    }
}
