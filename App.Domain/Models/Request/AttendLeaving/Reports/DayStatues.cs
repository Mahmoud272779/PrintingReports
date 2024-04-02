using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.Request.AttendLeaving.Reports
{
    public class DayStatues
    {
        public int? empId { get; set; }
        public string? branchId { get; set; }
        public string? debartmentId { get; set; }
        public string? sectionId { get; set; }
        public int? jobId { get; set; }
        public int? empGroupId { get; set; }
        public int? projectId { get; set; }
        public int? ShiftId { get; set; }
        [Required]
        public DateTime dateFrom { get; set; }
        [Required]
        public DateTime dateTo { get; set; }
    }
}
