using App.Domain.Entities.Process.AttendLeaving;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Org.BouncyCastle.Crypto.Tls;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Persistence.Configurations.Process.AttendLeaving
{
    internal class SectionsAndDepartmentsConfiguration : IEntityTypeConfiguration<SectionsAndDepartments>
    {
        public void Configure(EntityTypeBuilder<SectionsAndDepartments> builder)
        {
            builder.ToTable(nameof(SectionsAndDepartments));
            builder.HasKey(x => x.Id);
            builder.HasOne(c => c.emp).WithMany(c => c.sectionsAndDepartments).HasForeignKey(c => c.empId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
