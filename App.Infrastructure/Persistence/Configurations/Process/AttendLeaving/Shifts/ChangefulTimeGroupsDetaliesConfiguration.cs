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
    public class ChangefulTimeGroupsDetaliesConfiguration : IEntityTypeConfiguration<ChangefulTimeGroupsDetalies>
    {
        public void Configure(EntityTypeBuilder<ChangefulTimeGroupsDetalies> builder)
        {
            builder.ToTable(nameof(ChangefulTimeGroupsDetalies));
            builder.HasKey(x => x.Id);
            builder.HasOne(c=> c.changefulTimeGroups).WithMany(c=> c.changefulTimeGroups).HasForeignKey(c=> c.changefulTimeGroupsId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
