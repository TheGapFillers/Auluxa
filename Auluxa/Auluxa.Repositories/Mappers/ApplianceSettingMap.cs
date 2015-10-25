using System.Data.Entity.ModelConfiguration;
using Auluxa.Models;

namespace Auluxa.Repositories.Mappers
{
    public class ApplianceSettingMap : EntityTypeConfiguration<ApplianceSetting>
    {
        public ApplianceSettingMap()
        {
            ToTable("ApplianceSettings", "Auluxa");
            HasKey(a => a.Id);

            HasRequired(s => s.Appliance);
        }
    }
}
