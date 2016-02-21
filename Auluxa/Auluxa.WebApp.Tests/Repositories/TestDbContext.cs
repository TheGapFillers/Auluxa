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
		}


		public async Task<int> SaveChangesAsync()
		{
			return await Task.Factory.StartNew(() => 0);
		}

		public void Dispose() { }

		public class ApplicationDbContextInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
		{
			protected override void Seed(ApplicationDbContext context)
			{
				// Appliance Models
				var applianceModels = new List<ApplianceModel>
				{
					new ApplianceModel("A/C", "BrandNameA", "A/C A")
					{
						Id = 1,
						PossibleSettings = new Dictionary<string, string[]>
						{
							["FunctionA"] = new [] { "FunctionADefaultChoice", "FunctionAChoice2", "FunctionAChoice3" },
							["FunctionB"] = new [] { "FunctionBDefaultChoice", "FunctionBChoice2", "FunctionBChoice3" }
						}
					},
					new ApplianceModel("Light", "BrandNameA", "Light A")
					{
						Id = 2,
						PossibleSettings = new Dictionary<string, string[]>
						{
							["FunctionA"] = new [] { "FunctionADefaultChoice", "FunctionAChoice2", "FunctionAChoice3" },
							["FunctionB"] = new [] { "FunctionBDefaultChoice", "FunctionBChoice2", "FunctionBChoice3" }
						}
					}
				};
				context.ApplianceModels.AddRange(applianceModels);
				context.SaveChanges();

				// Appliances
				var appliances = new List<Appliance>
				{
					new Appliance
					{
						Id = 1,
						Name = "Appliance1",
						Model = applianceModels.Find(am => am.Id == 1),
						CurrentSetting =  new Dictionary<string, string>
						{
							["FunctionA"] = "FunctionAChoice2",
							["FunctionB"] = "FunctionBChoice3"
						}
					},
					new Appliance { Id = 2, Name = "Appliance2", Model = applianceModels.Find(am => am.Id == 2) },
				};
				context.Appliances.AddRange(appliances);
				context.SaveChanges();

				// Zones
				var zones = new List<Zone>
				{
					new Zone { Id = 1, Name = "Zone1" },
					new Zone
					{
						Id = 2,
						Name = "Bed Room",
						Appliances = new List<Appliance>
						{
							appliances.Find(a => a.Id == 1),
							appliances.Find(a => a.Id == 2)
						}
					}
				};
				context.Zones.AddRange(zones);
				context.SaveChanges();

				// Scenes
				var scenes = new List<Scene>
				{
					new Scene
					{
						Name = "EmptyScene",
						Schedule = new Schedule(),
						Sequence = new Sequence()
					},
					new Scene
					{
						Name = "Scene1",
						ApplianceSettings = new List<ApplianceSetting>
						{
							new ApplianceSetting
							{
								Appliance = appliances.Find(a => a.Id == 1), Setting = new Dictionary<string, string>
								{
									["FunctionA"] = "FunctionAChoice1",
									["FunctionB"] = "FunctionBChoice2"
								}
							},
							new ApplianceSetting { Appliance = appliances.Find(a => a.Id == 2) }
						},
						Schedule = new Schedule(),
						Sequence = new Sequence()
					}
				};
				context.Scenes.AddRange(scenes);
				context.SaveChanges();

				base.Seed(context);

				var settings = new Setting()
				{
					TimeZoneName = "Hong Kong",
					TimeZoneUtcOffset = 8
				};
				context.Settings.Add(settings);
				base.Seed(context);
				context.SaveChanges();

				var kranium = new Kranium()
				{
					Name = "One Kranium",
					Version = "0.1",
					IPAddress = "127.0.0.1" // new IPAddress(new byte[] { 127, 0, 0, 1 })
				};
				context.Kranium.Add(kranium);
				base.Seed(context);
				context.SaveChanges();
			}
		}
	}
}
