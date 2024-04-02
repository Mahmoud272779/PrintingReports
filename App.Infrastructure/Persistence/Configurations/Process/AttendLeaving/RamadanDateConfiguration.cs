using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Domain.Entities.Process.AttendLeaving;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infrastructure.Persistence.Configurations.Process.AttendLeaving
{
    public class RamadanDateConfiguration : IEntityTypeConfiguration<RamadanDate>
    {
        public void Configure(EntityTypeBuilder<RamadanDate> builder)
        {
            builder.ToTable(nameof(RamadanDate));
            builder.HasKey(p => p.id);
        }
    }
}
