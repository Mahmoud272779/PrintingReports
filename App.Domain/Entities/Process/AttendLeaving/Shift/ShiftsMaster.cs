using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Entities.Process.AttendLeaving
{
    public class ShiftsMaster
    {
        public int Id { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
        public DateTime dayEndTime { get; set; }
        public int shiftType { get; set; }
        public ICollection<NormalShiftDetalies> normalShiftDetalies { get; set; } // shift type 0 and 1
        public ICollection<ChangefulTimeGroupsMaster> changefulTimeGroups { get; set; } // shift type 2S
        public ICollection<InvEmployees> InvEmployees { get; set; }
    }
}
