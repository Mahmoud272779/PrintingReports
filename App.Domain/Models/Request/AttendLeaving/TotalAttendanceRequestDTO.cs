using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.Request.AttendLeaving
{
    public class TotalAttendanceRequestDTO
    {
        [Required]
        public string branchIds { get; set; }
        [Required]
        public DateTime dateFrom { get; set; }
        [Required]
        public DateTime dateTo { get; set; }
        public int? employeeId { get; set; }
        public string? managementIds { get; set; }
        public string? sectionsIds { get; set; }
        public int? projectId { get; set; }
        public int? jobId { get; set; }
        public int? employeeGroupId { get; set; }
        public int? shiftId { get; set; }
    }
}
