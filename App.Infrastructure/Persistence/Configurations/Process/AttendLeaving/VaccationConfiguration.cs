using App.Domain.Entities.Process.AttendLeaving;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Persistence.Configurations.Process.AttendLeaving
{
    public class VaccationConfiguration : IEntityTypeConfiguration<Vaccation>
    {
        public void Configure(EntityTypeBuilder<Vaccation> builder)
        {
            builder.ToTable(nameof(Vaccation));
            builder.HasKey(x => x.Id);
        }
    }
}
