using System;
using System.Data.Entity;
using Auluxa.Models;
using Auluxa.Repositories.Mappers;

namespace Auluxa.Repositories.Contexts
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext()
            : base("ApplicationConnection")
        {
            Configuration.LazyLoadingEnabled = false;                           // Disable lazy loading for all db sets.
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ApplicationDbContext>());     // No code first initialisation.
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
    }
}
