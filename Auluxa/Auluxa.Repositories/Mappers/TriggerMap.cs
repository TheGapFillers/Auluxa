using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Auluxa.Models;

namespace Auluxa.Repositories.Mappers
{
    public class TriggerMap : EntityTypeConfiguration<Trigger>
    {
        public TriggerMap()
        {
            ToTable("Triggers", "Auluxa");
            HasKey(t => t.Id);
            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(t => t.Appliance);
        }
    }
}
