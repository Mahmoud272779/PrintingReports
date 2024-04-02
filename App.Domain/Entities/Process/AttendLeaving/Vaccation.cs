using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Entities.Process.AttendLeaving
{
    public class Vaccation
    {
        public int Id { get; set; }
        public string ArabicName { get; set; }
        public string LatinName { get; set; }

        public ICollection<VaccationEmployees> vaccationEmployees { get; set; }

    }
}
