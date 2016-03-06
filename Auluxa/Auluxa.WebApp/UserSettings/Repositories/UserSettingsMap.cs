using System.Data.Entity.ModelConfiguration;
using Auluxa.WebApp.UserSettings.Models;

namespace Auluxa.WebApp.UserSettings.Repositories
{
	public class UserSettingsMap : EntityTypeConfiguration<UserSetting>
	{
		public UserSettingsMap()
		{
			ToTable("Settings", "Auluxa");
			HasKey(s => s.Id);
		}
	}
}
