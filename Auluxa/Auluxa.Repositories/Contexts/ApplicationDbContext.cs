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
        public DbSet<ApplianceModel> ApplianceModels { get; set; }
        public DbSet<Appliance> Appliances { get; set; }
        public DbSet<ApplianceSetting> ApplianceSettings { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<Trigger> Triggers { get; set; }

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
                            ["FunctionA"] = new [] { "FunctionAChoice1", "FunctionAChoice2", "FunctionAChoice3" },
                            ["FunctionB"] = new [] { "FunctionBChoice1", "FunctionBChoice2", "FunctionBChoice3" }
                        }
                    },
                    new ApplianceModel("Light", "BrandNameA", "Light A")
                    {
                        Id = 2,
                        PossibleSettings = new Dictionary<string, string[]>
                        {
                            ["FunctionA"] = new [] { "FunctionAChoice1", "FunctionAChoice2", "FunctionAChoice3" },
                            ["FunctionB"] = new [] { "FunctionBChoice1", "FunctionBChoice2", "FunctionBChoice3" }
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
            }
        }
    }
}
