using App.Domain.Entities.Process.AttendLeaving.Shift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Entities.Process.AttendLeaving
{
    public class ChangefulTimeGroupsDetalies : TimesComman
    {
        public int Id { get; set; }
        public int workDaysNumber { get; set; }
        public int weekendNumber { get; set; }
        public bool IsRamadan { get; set; }
        public int changefulTimeGroupsId { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public ChangefulTimeGroupsMaster changefulTimeGroups { get; set; }
    }
}
