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
    public class ChangefulTimeGroupsMasterConfiguration : IEntityTypeConfiguration<ChangefulTimeGroupsMaster>
    {
        public void Configure(EntityTypeBuilder<ChangefulTimeGroupsMaster> builder)
        {
            builder.ToTable(nameof(ChangefulTimeGroupsMaster));
            builder.HasKey(x => x.Id);
            builder.HasOne(c=> c.shiftsMaster).WithMany(c=> c.changefulTimeGroups).HasForeignKey(c=> c.shiftsMasterId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
