using System.Data.Entity.ModelConfiguration;
using Auluxa.WebApp.Models;

namespace Auluxa.WebApp.Repositories.Mappers
{
    public class SequenceMap : EntityTypeConfiguration<Sequence>
    {
        public SequenceMap()
        {
            ToTable("Scenes", "Auluxa");

            HasMany(s => s.Triggers)
                .WithRequired(t => t.Sequence);
        }
    }
}
