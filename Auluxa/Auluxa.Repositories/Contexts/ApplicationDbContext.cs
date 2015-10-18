using System;
using System.Data.Entity;
using Auluxa.Models;
using Auluxa.Repositories.Mappers;
using System.Collections.Generic;

namespace Auluxa.Repositories.Contexts
{
	public class ApplicationDbContext : DbContext, IApplicationDbContext
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
		public DbSet<Appliance> Appliances { get; set; }
		public DbSet<ApplianceSetting> ApplianceSettings { get; set; }
		public DbSet<Zone> Zones { get; set; }
		public DbSet<Trigger> Triggers { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));

			modelBuilder.Configurations.Add(new ApplianceMap());
			modelBuilder.Configurations.Add(new ApplianceSettingMap());
			modelBuilder.Configurations.Add(new SceneMap());
			modelBuilder.Configurations.Add(new ScheduleMap());
			modelBuilder.Configurations.Add(new SequenceMap());
			modelBuilder.Configurations.Add(new TriggerMap());
			modelBuilder.Configurations.Add(new ZoneMap());
		}

		public class ApplicationDbContextInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
		{
			protected override void Seed(ApplicationDbContext context)
			{

				var appliances = new List<Appliance>
				{
					new Appliance
					{
						Name = "Appliance1"
					},
					new Appliance
					{
						Name = "Appliance2"
					},
				};
				context.Appliances.AddRange(appliances);
				context.SaveChanges();

				var zones = new List<Zone>
				{
					new Zone { Name = "Zone1", Appliances = appliances },
					new Zone
					{
						Name = "Bed Room",
						Appliances = new List<Appliance>
						{
							new Appliance
							{
								Name = "BT Speaker"
							},
							new Appliance
							{
								Name = "HDTV"
							},
						}
					}
				};
				context.Zones.AddRange(zones);
				context.SaveChanges();

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
							new ClimateSetting { Appliance = context.Appliances.SingleAsync(a => a.Name == "Appliance1").Result },
							new LightSetting { Appliance = context.Appliances.SingleAsync(a => a.Name == "Appliance2").Result }
						},
						Schedule = new Schedule(),
						Sequence = new Sequence()
					}
				};
				context.Scenes.AddRange(scenes);
				context.SaveChanges();




				//Scene sceneToUpdate = context.Scenes.SingleAsync(s => s.Name == "Scene1").Result;
				//sceneToUpdate.ApplianceSettings = new List<ApplianceSetting>
				//{
				//    new ClimateSetting { Appliance = appliances.Find(a => a.ApplianceId == 0) },
				//    new LightSetting { Appliance = appliances.Find(a => a.ApplianceId == 1) }
				//};
				//context.SaveChanges();

				base.Seed(context);
			}
		}
	}
}
