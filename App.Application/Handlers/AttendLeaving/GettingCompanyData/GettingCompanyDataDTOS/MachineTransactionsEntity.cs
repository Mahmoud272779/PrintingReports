using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.GettingCompanyData.GettingCompanyDataDTOS
{
    public class MachineTransactionsEntity
    {
        public int Id { get; set; }
        public int EmployeeID { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime PushTime { get; set; }
        public string MachineSN { get; set; }
        public bool IsMoved { get; set; }
        public bool IsEdited { get; set; }
    }
}
