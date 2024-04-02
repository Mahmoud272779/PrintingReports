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
    public class religionsConfiguration : IEntityTypeConfiguration<religions>
    {
        public void Configure(EntityTypeBuilder<religions> builder)
        {
            builder.ToTable(nameof(religions));
            builder.HasKey(x => x.Id);
        }
    }
}
