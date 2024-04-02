using App.Domain.Entities.Process;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Persistence.Configurations.Process
{
    public class GLJournalEntryDetailsAccountsConfiguaratons : IEntityTypeConfiguration<GLJournalEntryDetailsAccounts>
    {
        public void Configure(EntityTypeBuilder<GLJournalEntryDetailsAccounts> builder)
        {
            builder.ToTable("GLJournalEntryDetailsAccounts");
            builder.HasKey(c => new { c.JournalEntryId, c.FinancialAccountId });

            builder.HasOne(e => e.FinancialAccount)
            .WithMany(d => d.journalEntryDetailsAccounts)
            .HasForeignKey(q => q.FinancialAccountId);


            builder.HasOne(e => e.JournalEntry)
            .WithMany(d => d.JournalEntryDetailsAccounts)
            .HasForeignKey(q => q.JournalEntryId);

        }
    }
}
