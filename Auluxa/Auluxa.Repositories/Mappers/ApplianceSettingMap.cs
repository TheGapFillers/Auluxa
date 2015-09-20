using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Auluxa.Models;

namespace Auluxa.Repositories.Mappers
{
    public class ApplianceSettingMap : EntityTypeConfiguration<ApplianceSetting>
    {
        public ApplianceSettingMap()
        {
            ToTable("Appliances", "Auluxa");
            HasKey(a => a.Id);

            Map<ClimateSetting>(m => m.Requires("Type").HasValue("ClimateSetting"));
            Map<LightSetting>(m => m.Requires("Type").HasValue("LightSetting"));
            Map<ShadeSetting>(m => m.Requires("Type").HasValue("ShadeSetting"));
        }
    }
}
