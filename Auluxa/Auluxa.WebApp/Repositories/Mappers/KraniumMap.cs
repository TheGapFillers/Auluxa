using System.Data.Entity.ModelConfiguration;
using Auluxa.WebApp.Models;

namespace Auluxa.WebApp.Repositories.Mappers
{
	public class KraniumMap : EntityTypeConfiguration<Kranium>
	{
		public KraniumMap()
		{
			ToTable("Kranium", "Auluxa");
			HasKey(k => k.Id);
		}
	}
}
