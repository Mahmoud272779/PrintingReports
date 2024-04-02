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
    public class AttendLeaving_SettingsConfiguration : IEntityTypeConfiguration<AttendLeaving_Settings>
    {
        public void Configure(EntityTypeBuilder<AttendLeaving_Settings> builder)
        {
            builder.ToTable(nameof(AttendLeaving_Settings));
            builder.HasKey(c => c.Id);
            builder.HasOne(c => c.GLBranch).WithMany(c => c.AttendLeaving_Settings).HasForeignKey(c => c.BranchId).OnDelete(DeleteBehavior.NoAction);
            builder.Property(c => c.BranchId).HasDefaultValue(1);
        }
    }
}
