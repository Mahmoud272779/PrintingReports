using App.Domain.Entities.Process.AttendLeaving;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Org.BouncyCastle.Asn1.X509.Qualified;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Persistence.Configurations.Process.AttendLeaving.Shifts
{
    public class NormalShiftDetaliesConfiguration : IEntityTypeConfiguration<NormalShiftDetalies>
    {
        public void Configure(EntityTypeBuilder<NormalShiftDetalies> builder)
        {
            builder.ToTable(nameof(NormalShiftDetalies));
            builder.HasKey(x => x.Id);
            builder.HasOne(c => c.Shift).WithMany(c => c.normalShiftDetalies).HasForeignKey(c => c.ShiftId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
