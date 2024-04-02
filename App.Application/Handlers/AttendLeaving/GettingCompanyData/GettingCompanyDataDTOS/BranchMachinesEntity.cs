using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.GettingCompanyData.GettingCompanyDataDTOS
{
    public class BranchMachinesEntity
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public int BranchId { get; set; }
        public string MachineNameAr { get; set; }
        public string MachineNameEn { get; set; }
        public string MachineSN { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public string CreateUserId { get; set; }
        public string ModifyUserId { get; set; }
        public string Status { get; set; }
    }
}
