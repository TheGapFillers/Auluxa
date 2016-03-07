using System.Data.Entity.ModelConfiguration;
using Auluxa.WebApp.Scenes.Models;

namespace Auluxa.WebApp.Scenes.Repositories
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
