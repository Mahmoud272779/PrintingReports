using App.Domain.Entities.Process.AttendLeaving.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Persistence.Configurations.Process.AttendLeaving.Transactions
{
    public class MachineTransactionsConfiguration : IEntityTypeConfiguration<MachineTransactions>
    {
        public void Configure(EntityTypeBuilder<MachineTransactions> builder)
        {
            builder.ToTable(nameof(MachineTransactions));
            builder.HasKey(x => x.Id);
            builder.HasOne(c => c.machine).WithMany(c => c.MachineTransactions).HasForeignKey(c => c.machineId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
