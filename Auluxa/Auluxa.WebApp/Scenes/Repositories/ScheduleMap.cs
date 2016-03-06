using System.Data.Entity.ModelConfiguration;
using Auluxa.WebApp.Scenes.Models;

namespace Auluxa.WebApp.Scenes.Repositories
{
    public class ScheduleMap : EntityTypeConfiguration<Schedule>
    {
        public ScheduleMap()
        {
            ToTable("Scenes", "Auluxa");
        }
    }
}
