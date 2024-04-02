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
    public class EmployeesGroupConfiguration : IEntityTypeConfiguration<EmployeesGroup>
    {
        public void Configure(EntityTypeBuilder<EmployeesGroup> builder)
        {
            builder.ToTable(nameof(EmployeesGroup));
            builder.HasKey(p => p.Id);
        }
    }
}
