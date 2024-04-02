using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.Response.HR.AttendLeaving
{
    public class VaccationResponseDTO
    {
        public int Id { get; set; }
        public string ArabicName { get; set; }
        public string LatinName { get; set; }
        public bool candelete { get; set; }
    }
}
