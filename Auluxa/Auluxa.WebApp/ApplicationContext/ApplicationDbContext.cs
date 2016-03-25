using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace Auluxa.WebApp.ApplicationContext
{
	public class ApplicationDbContext : DbContext, IApplianceDbContext, IKraniumDbContext, ISceneDbContext, IUserSettingsDbContext, IZoneDbContext
	{
		public ApplicationDbContext()
			: base("ApplicationConnection")
		{
			Configuration.LazyLoadingEnabled = false;                           // Disable lazy loading for all db sets.
			Database.SetInitializer(new ApplicationDbContextInitializer());     // No code first initialisation.
		}

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

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));

			modelBuilder.Configurations.Add(new ApplianceModelMap());
			modelBuilder.Configurations.Add(new ApplianceMap());
			modelBuilder.Configurations.Add(new ApplianceSettingMap());
			modelBuilder.Configurations.Add(new SceneMap());
			modelBuilder.Configurations.Add(new ScheduleMap());
			modelBuilder.Configurations.Add(new SequenceMap());
			modelBuilder.Configurations.Add(new TriggerMap());
			modelBuilder.Configurations.Add(new ZoneMap());
			modelBuilder.Configurations.Add(new UserSettingsMap());
			modelBuilder.Configurations.Add(new KraniumMap());
		}

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
						UserName = "Serge",
						Model = applianceModels.Find(am => am.Id == 1),
						CurrentSetting =  new Dictionary<string, string>
						{
							["FunctionA"] = "FunctionAChoice2",
							["FunctionB"] = "FunctionBChoice3"
						}
					},
					new Appliance {
						Id = 2,
						Name = "Appliance2",
						UserName = "Marcel",
						Model = applianceModels.Find(am => am.Id == 2)
					},
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

				var settings = new UserSetting()
				{
					TimeZoneName = "Hong Kong",
					TimeZoneUtcOffset = 8
				};
				context.Settings.Add(settings);
				base.Seed(context);
				context.SaveChanges();

				var kranium = new KraniumEntity()
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
