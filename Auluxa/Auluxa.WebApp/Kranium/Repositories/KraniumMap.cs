using System.Data.Entity.ModelConfiguration;
using Auluxa.WebApp.Kranium.Models;

namespace Auluxa.WebApp.Kranium.Repositories
{
	public class KraniumMap : EntityTypeConfiguration<KraniumEntity>
	{
		public KraniumMap()
		{
			ToTable("Kranium", "Auluxa");
			HasKey(k => k.Id);
		}
	}
}
