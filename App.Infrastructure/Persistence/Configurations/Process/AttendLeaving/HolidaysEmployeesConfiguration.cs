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
    public class HolidaysEmployeesConfiguration : IEntityTypeConfiguration<HolidaysEmployees>
    {

        public void Configure(EntityTypeBuilder<HolidaysEmployees> builder)
        {
            builder.ToTable(nameof(HolidaysEmployees));
            builder.HasKey(x => x.Id);
            builder.HasOne(c => c.Employees).WithMany(c => c.EmployeesHolidays).HasForeignKey(c => c.EmployeesId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(c => c.Holidays).WithMany(c => c.EmployeesHolidays).HasForeignKey(c => c.HolidaysId).OnDelete(DeleteBehavior.NoAction);
        }

    }
}
