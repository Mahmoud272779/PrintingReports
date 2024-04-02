using App.Domain.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.Request.AttendLeaving
{
    public class ChangefulTime_Employees_EditDTO
    {
        public int groupId { get; set; }
        public string employeesId { get; set; }
    }
    public class ChangefulTime_GetEmpsRequest_DTO : PaginationVM
    {
        public string? SearchCriteria { get; set; }
        [Required]
        public int GroupId { get; set; }
    }

}
