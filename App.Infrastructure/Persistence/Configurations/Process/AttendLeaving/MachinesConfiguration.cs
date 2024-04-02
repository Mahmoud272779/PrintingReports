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
    public class MachinesConfiguration : IEntityTypeConfiguration<Machines>
    {
        public void Configure(EntityTypeBuilder<Machines> builder)
        {
            builder.ToTable(nameof(Machines));
            builder.HasKey(x => x.Id);
            builder.HasOne(c=> c.branch).WithMany(c=> c.Machines).HasForeignKey(c=>c.branchId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
