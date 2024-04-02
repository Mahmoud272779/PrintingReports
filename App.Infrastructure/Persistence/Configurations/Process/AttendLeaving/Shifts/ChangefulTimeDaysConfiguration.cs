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
    public class ChangefulTimeDaysConfiguration : IEntityTypeConfiguration<ChangefulTimeDays>
    {
        public void Configure(EntityTypeBuilder<ChangefulTimeDays> builder)
        {
            builder.ToTable(nameof(ChangefulTimeDays));
            builder.HasKey(x => x.Id);
            builder.HasOne(c => c.changefulTimeGroups).WithMany(c => c.changefulTimeDays).HasForeignKey(c => c.changefulTimeGroupsId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
