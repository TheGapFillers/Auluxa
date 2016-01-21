using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Auluxa.WebApp.Models;

namespace Auluxa.WebApp.Repositories.Mappers
{
	public class SettingsMap : EntityTypeConfiguration<Settings>
	{
		public SettingsMap()
		{
			ToTable("Settings", "Auluxa");
			HasKey(s => s.Id);
		}
	}
}
