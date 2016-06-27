using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Auluxa.WebApp.Scenes.Models;

namespace Auluxa.WebApp.Scenes.Repositories
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

            HasMany(s => s.DeviceSettings);

            //HasMany(s => s.DeviceSettings)
            //    .WithMany()
            //    .Map(m =>
            //    {
            //        m.ToTable("SceneDeviceSettings", "Auluxa");
            //        m.MapLeftKey("SceneId");
            //        m.MapRightKey("DeviceSettingId");
            //    });
        }
    }
}
