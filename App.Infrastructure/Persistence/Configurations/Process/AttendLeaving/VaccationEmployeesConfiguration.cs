using App.Domain.Entities.Process.AttendLeaving;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Persistence.Configurations.Process.AttendLeaving
{
    public class VaccationEmployeesConfiguration : IEntityTypeConfiguration<VaccationEmployees>
    {

        public void Configure(EntityTypeBuilder<VaccationEmployees> builder)
        {
            builder.ToTable(nameof(VaccationEmployees));
            builder.HasKey(x => x.Id);
            builder.HasOne(c => c.Employees).WithMany(c => c.VaccationEmployees).HasForeignKey(c => c.EmployeeId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(c => c.Vaccations).WithMany(c => c.vaccationEmployees).HasForeignKey(c => c.VaccationId).OnDelete(DeleteBehavior.NoAction);
        }

    }
}
