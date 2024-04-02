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
    public class AttendancPermissionPermissionConfigurations : IEntityTypeConfiguration<AttendancPermission>
    {
        public void Configure(EntityTypeBuilder<AttendancPermission> builder)
        {
            builder.ToTable(nameof(AttendancPermission));
            builder.HasKey(x => x.Id);
            builder.HasOne(c => c.employees).WithMany(c => c.permissions).HasForeignKey(c => c.EmpId).OnDelete(DeleteBehavior.NoAction);
            
        }
    }
}
