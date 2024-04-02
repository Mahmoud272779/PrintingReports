using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.Request.AttendLeaving.Reports
{
    public class DetailedAttendanceRequestDTO
    {
        public int? empId { get; set; }
        public int? branchId { get; set; }
        public int? debartmentId { get; set; }
        public int? sectionId { get; set; }
        public int? jobId { get; set; }
        public int? empGroupId { get; set; }
        public int? ShiftId { get; set; }
        [Required]
        public DateTime dateFrom { get; set; }
        [Required]
        public DateTime dateTo { get; set; }
    }
}
