using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Auluxa.WebApp.Models;

namespace Auluxa.WebApp.Repositories.Mappers
{
    public class ApplianceModelMap : EntityTypeConfiguration<ApplianceModel>
    {
        public ApplianceModelMap()
        {
            ToTable("ApplianceModels", "Auluxa");
            HasKey(a => a.Id);
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);         
        }
    }
}
