using App.Domain.Entities.Process.AttendLeaving.Shift;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Persistence.Configurations.Process.AttendLeaving.Shifts
{
    public class ChangefulTimeGroupsEmployeesConfiguration : IEntityTypeConfiguration<ChangefulTimeGroupsEmployees>
    {
        public void Configure(EntityTypeBuilder<ChangefulTimeGroupsEmployees> builder)
        {
            builder.ToTable(nameof(ChangefulTimeGroupsEmployees));
            builder.HasKey(e => e.Id);
            builder.HasOne(c => c.changefulTimeGroupsMaster).WithMany(c => c.changefulTimeGroupsEmployees).HasForeignKey(c => c.changefulTimeGroupsMasterId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(c => c.invEmployees).WithMany(c => c.ChangefulTimeGroupsEmployees).HasForeignKey(c => c.invEmployeesId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
