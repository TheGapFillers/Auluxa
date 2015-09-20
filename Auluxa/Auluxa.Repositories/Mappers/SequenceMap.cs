using System.Data.Entity.ModelConfiguration;
using Auluxa.Models;

namespace Auluxa.Repositories.Mappers
{
    public class SequenceMap : EntityTypeConfiguration<Sequence>
    {
        public SequenceMap()
        {
            ToTable("Scenes", "Auluxa");
            
            HasMany(s => s.Triggers)
                .WithMany()
                .Map(m =>
                {
                    m.ToTable("SequenceTriggers");
                    m.MapLeftKey("SequenceId");
                    m.MapRightKey("TriggerId");
                });
        }
    }
}
