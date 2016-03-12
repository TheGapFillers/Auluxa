using System.Data.Entity;
using System.Threading.Tasks;
using Auluxa.WebApp.Appliances.Models;
using Auluxa.WebApp.Appliances.Repositories;
using Auluxa.WebApp.Kranium.Models;
using Auluxa.WebApp.Kranium.Repositories;
using Auluxa.WebApp.Scenes.Models;
using Auluxa.WebApp.Scenes.Repositories;
using Auluxa.WebApp.UserSettings.Models;
using Auluxa.WebApp.UserSettings.Repositories;
using Auluxa.WebApp.Zones.Models;
using Auluxa.WebApp.Zones.Repositories;
using Auluxa.WebApp.Tests.Repositories;

namespace Auluxa.WebApp.Tests
{
	public class TestDbContext : IApplianceDbContext, IKraniumDbContext, ISceneDbContext, IUserSettingsDbContext, IZoneDbContext
	{
		public DbSet<Scene> Scenes { get; set; }
		public DbSet<Sequence> Sequences { get; set; }
		public DbSet<Schedule> Schedules { get; set; }
		public DbSet<ApplianceModel> ApplianceModels { get; set; }
		public DbSet<Appliance> Appliances { get; set; }
		public DbSet<ApplianceSetting> ApplianceSettings { get; set; }
		public DbSet<Zone> Zones { get; set; }
		public DbSet<Trigger> Triggers { get; set; }
		public DbSet<UserSetting> Settings { get; set; }
		public DbSet<KraniumEntity> Kranium { get; set; }

		public TestDbContext()
		{
			Zones = new TestDbSet<Zone>();
			ApplianceModels = new TestDbSet<ApplianceModel>();
			Appliances = new TestDbSet<Appliance>();
		}

		public async Task<int> SaveChangesAsync()
		{
			return await Task.Factory.StartNew(() => 0);
		}

		public void Dispose() { }
	}
}
