using System;
using System.Collections.Generic;
using System.Data.Entity;
using Auluxa.WebApp.Models;
using Auluxa.WebApp.Repositories.Contexts;
using System.Threading.Tasks;
using Auluxa.WebApp.Tests.Repositories;

namespace Auluxa.WebApp.Tests
{
	public class TestDbContext : IApplicationDbContext
	{
		public DbSet<Scene> Scenes { get; set; }
		public DbSet<Sequence> Sequences { get; set; }
		public DbSet<Schedule> Schedules { get; set; }
		public DbSet<ApplianceModel> ApplianceModels { get; set; }
		public DbSet<Appliance> Appliances { get; set; }
		public DbSet<ApplianceSetting> ApplianceSettings { get; set; }
		public DbSet<Zone> Zones { get; set; }
		public DbSet<Trigger> Triggers { get; set; }
		public DbSet<Setting> Settings { get; set; }
		public DbSet<Kranium> Kranium { get; set; }

		public TestDbContext()
		{
			Zones = new TestZoneDbSet();
			ApplianceModels = new TestApplianceModelDbSet();
			Appliances = new TestApplianceDbSet();
		}


		public async Task<int> SaveChangesAsync()
		{
			return await Task.Factory.StartNew(() => 0);
		}

		public void Dispose() { }
	}
}
