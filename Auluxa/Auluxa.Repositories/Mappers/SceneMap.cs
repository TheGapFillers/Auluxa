using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Auluxa.Models;

namespace Auluxa.Repositories.Mappers
{
    public class SceneMap : EntityTypeConfiguration<Scene>
    {
        public SceneMap()
        {
            ToTable("Scenes", "Auluxa");
            HasKey(s => s.Id);
            Property(s => s.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(s => s.Schedule)
                .WithRequiredPrincipal(s => s.Scene);
            HasRequired(s => s.Sequence)
                .WithRequiredPrincipal(s => s.Scene);

            HasMany(s => s.ApplianceSettings);

            //HasMany(s => s.ApplianceSettings)
            //    .WithMany()
            //    .Map(m =>
            //    {
            //        m.ToTable("SceneApplianceSettings", "Auluxa");
            //        m.MapLeftKey("SceneId");
            //        m.MapRightKey("ApplianceSettingId");
            //    });
        }
    }
}
