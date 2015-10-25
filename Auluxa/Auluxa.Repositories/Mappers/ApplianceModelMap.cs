using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auluxa.Models;

namespace Auluxa.Repositories.Mappers
{
    public class ApplianceModelMap : EntityTypeConfiguration<ApplianceModel>
    {
        public ApplianceModelMap()
        {
            ToTable("ApplianceModels", "Auluxa");
            HasKey(a => a.Id);
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);         
        }
    }
}
