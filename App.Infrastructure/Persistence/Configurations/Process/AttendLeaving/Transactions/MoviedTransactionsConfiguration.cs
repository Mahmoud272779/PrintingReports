using App.Domain.Entities.Process.AttendLeaving.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Persistence.Configurations.Process.AttendLeaving.Transactions
{
    public class MoviedTransactionsConfiguration : IEntityTypeConfiguration<MoviedTransactions>
    {
        public void Configure(EntityTypeBuilder<MoviedTransactions> builder)
        {
            builder.ToTable(nameof(MoviedTransactions));
            builder.HasKey(x => x.Id);
            builder.HasOne(c=> c.Employees).WithMany(c=> c.MoviedTransactions).HasForeignKey(c => c.EmployeesId).OnDelete(DeleteBehavior.NoAction);
            builder.Property(c => c.IsHaveShift2).HasDefaultValue(false);
            builder.Property(c => c.IsHaveShift3).HasDefaultValue(false);
            builder.Property(c => c.IsHaveShift4).HasDefaultValue(false);
            builder.Property(c => c.IsEdited).HasDefaultValue(false);
            builder.Property(c => c.IsAbsance).HasDefaultValue(false);
            builder.Property(c => c.IsHoliday).HasDefaultValue(false);
        }
    }
}
