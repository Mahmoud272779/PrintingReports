using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Entities.Process.AttendLeaving
{
    public class RamadanDate
    {
        public int id { get; set; }
        
        public string ArabicName { get; set; }
        public string LatinName { get; set; }
        
        public DateTime FromDate { get; set; }
        
        public DateTime ToDate { get; set; }
        public string Note { get; set; }
    }
}
