using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Entities.Process
{
    public class GLJournalEntryDetailsAccounts
    {
        public int FinancialAccountId { get; set; }
        public int JournalEntryId { get; set; }

        public virtual GLFinancialAccount FinancialAccount { get; set; }
        public virtual GLJournalEntry JournalEntry { get; set; }

    }
}
