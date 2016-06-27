using System;
using System.Collections.Generic;
using System.Data.Entity;
using Auluxa.WebApp.Devices.Models;
using Auluxa.WebApp.Devices.Repositories;
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
    public class ApplicationDbContext : DbContext, IDeviceDbContext, IKraniumDbContext, ISceneDbContext, IUserSettingsDbContext, IZoneDbContext
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
        public DbSet<DeviceModel> DeviceModels { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceSetting> DeviceSettings { get; set; }
        public DbSet<Zone> Zones { get; set; }
        //public DbSet<Trigger> Triggers { get; set; }
        public DbSet<UserSetting> Settings { get; set; }
        public DbSet<KraniumEntity> Kranium { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));

            modelBuilder.Configurations.Add(new DeviceModelMap());
            modelBuilder.Configurations.Add(new DeviceMap());
            modelBuilder.Configurations.Add(new DeviceSettingMap());
            modelBuilder.Configurations.Add(new SceneMap());
            modelBuilder.Configurations.Add(new ScheduleMap());
            modelBuilder.Configurations.Add(new SequenceMap());
            //modelBuilder.Configurations.Add(new TriggerMap());
            modelBuilder.Configurations.Add(new ZoneMap());
            modelBuilder.Configurations.Add(new UserSettingsMap());
            modelBuilder.Configurations.Add(new KraniumMap());
        }

        public class ApplicationDbContextInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
        {
            protected override void Seed(ApplicationDbContext context)
            {
                // Zones
                var zones = new List<Zone>
                {
                    new Zone { Id = 1, Name = "Zone1", UserName = "ambroise.couissin@gmail.com"},
                    new Zone
                    {
                        Id = 2,
                        Name = "Bed Room",
                        UserName = "ambroise.couissin@gmail.com"
                    }
                };
                context.Zones.AddRange(zones);
                context.SaveChanges();

                // Device Models
                var deviceModels = new List<DeviceModel>
                {
                    new DeviceModel("A/C", "BrandNameA", "A/C A")
                    {
                        Id = 1,
                        PossibleSettings = new Dictionary<string, string[]>
                        {
                            ["ACFunctionA"] = new [] { "FunctionADefaultChoice", "FunctionAChoice2", "FunctionAChoice3" },
                            ["ACFunctionB"] = new [] { "FunctionBDefaultChoice", "FunctionBChoice2", "FunctionBChoice3" }
                        }
                    },
                    new DeviceModel("Light", "BrandNameA", "Light A")
                    {
                        Id = 2,
                        PossibleSettings = new Dictionary<string, string[]>
                        {
                            ["LightFunctionA"] = new [] { "FunctionADefaultChoice", "FunctionAChoice2", "FunctionAChoice3" },
                            ["LightFunctionB"] = new [] { "FunctionBDefaultChoice", "FunctionBChoice2", "FunctionBChoice3" }
                        }
                    },
                    new DeviceModel("Switch", "BrandNameA", "Switch A")
                    {
                        Id = 2,
                        PossibleSettings = new Dictionary<string, string[]>
                        {
                            ["SwitchFunctionA"] = new [] { "FunctionADefaultChoice", "FunctionAChoice2", "FunctionAChoice3" },
                            ["SwitchFunctionB"] = new [] { "FunctionBDefaultChoice", "FunctionBChoice2", "FunctionBChoice3" }
                        }
                    }
                };
                context.DeviceModels.AddRange(deviceModels);
                context.SaveChanges();

                // Devices
                var devices = new List<Device>
                {
                    new Device
                    {
                        Id = 1,
                        Name = "Device1",
                        UserName = "ambroise.couissin@gmail.com",
                        Model = deviceModels.Find(am => am.Id == 1),
                        CurrentSetting =  new Dictionary<string, string>
                        {
                            ["ACFunctionA"] = "FunctionAChoice2",
                            ["ACFunctionB"] = "FunctionBChoice3"
                        }
                    },
                    new Device {
                        Id = 2,
                        Name = "Device2",
                        UserName = "ambroise.couissin@gmail.com",
                        Model = deviceModels.Find(am => am.Id == 2)
                    },
                };
                context.Devices.AddRange(devices);
                context.SaveChanges();

                // Scenes
                var scenes = new List<Scene>
                {
                    new Scene
                    {
                        UserName = "ambroise.couissin@gmail.com",
                        Name = "EmptyScene",
                        Schedule = new Schedule(),
                        Sequence = new Sequence()
                    },
                    new Scene
                    {
                        Name = "Scene1",
                        UserName = "ambroise.couissin@gmail.com",
                        DeviceSettings = new List<DeviceSetting>
                        {
                            new DeviceSetting
                            {
                                Device = devices.Find(a => a.Id == 1), Setting = new Dictionary<string, string>
                                {
                                    ["LightFunctionA"] = "FunctionAChoice1",
                                    ["LightFunctionB"] = "FunctionBChoice2"
                                }
                            },
                            new DeviceSetting { Device = devices.Find(a => a.Id == 2) }
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
