using App.Domain.Entities.Process.AttendLeaving;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Org.BouncyCastle.Asn1.X509.Qualified;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Persistence.Configurations.Process.AttendLeaving
{
    public class MissionsConfiguration : IEntityTypeConfiguration<Missions>
    {
        public void Configure(EntityTypeBuilder<Missions> builder)
        {
            builder.ToTable(nameof(Missions));
            builder.HasKey(x => x.Id);
        }
    }
}
