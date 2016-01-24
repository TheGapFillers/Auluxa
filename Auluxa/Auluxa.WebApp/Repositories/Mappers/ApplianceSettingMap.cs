using System.Data.Entity.ModelConfiguration;
using Auluxa.WebApp.Models;

namespace Auluxa.WebApp.Repositories.Mappers
{
    public class ApplianceSettingMap : EntityTypeConfiguration<ApplianceSetting>
    {
        public ApplianceSettingMap()
        {
            ToTable("ApplianceSettings", "Auluxa");
            HasKey(a => a.Id);

            //HasRequired(s => s.Appliance);
        }
    }
}
