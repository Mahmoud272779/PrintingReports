using App.Domain.Entities.Process.AttendLeaving.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Entities.Process.AttendLeaving
{
    public class Machines
    {
        public int Id { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
        public string MachineSN { get; set; }
        public int branchId { get; set; }
        public GLBranch branch { get; set; }
        public ICollection<MachineTransactions> MachineTransactions { get; set; }
        public ICollection<InvEmployees> InvEmployees { get; set; }
    }
}
