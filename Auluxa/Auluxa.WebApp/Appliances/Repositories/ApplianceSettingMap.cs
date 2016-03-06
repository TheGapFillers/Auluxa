using System.Data.Entity.ModelConfiguration;
using Auluxa.WebApp.Appliances.Models;

namespace Auluxa.WebApp.Appliances.Repositories
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
