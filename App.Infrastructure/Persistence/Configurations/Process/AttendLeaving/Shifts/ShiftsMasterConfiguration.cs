using App.Domain.Entities.Process.AttendLeaving;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Persistence.Configurations.Process.AttendLeaving.Shifts
{
    public class ShiftsMasterConfiguration : IEntityTypeConfiguration<ShiftsMaster>
    {
        public void Configure(EntityTypeBuilder<ShiftsMaster> builder)
        {
            builder.ToTable(nameof(ShiftsMaster));
            builder.HasKey(x => x.Id);
        }
    }
}
