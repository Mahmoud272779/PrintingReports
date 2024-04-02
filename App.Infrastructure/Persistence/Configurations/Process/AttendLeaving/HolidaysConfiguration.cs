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
    public class HolidaysConfiguration : IEntityTypeConfiguration<Holidays>
    {
        public void Configure(EntityTypeBuilder<Holidays> builder)
        {
            builder.ToTable(nameof(Holidays));
            builder.HasKey(x => x.Id);
        }
    }
}
