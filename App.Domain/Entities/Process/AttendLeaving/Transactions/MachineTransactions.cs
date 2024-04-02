using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Entities.Process.AttendLeaving.Transactions
{
    public class MachineTransactions
    {
        public int Id { get; set; }
        public int EmployeeCode {get;set;}
        public DateTime TransactionDate {get;set;}
        public DateTime EditedTransactionDate {get;set;}
        public int machineId {get;set;}
        public bool IsMoved {get;set;}
        public bool isAuto {get;set;}
        public bool IsEdited {get;set;}
        public DateTime PushTime {get;set;}
        public Machines machine { get; set; }

    }
}
