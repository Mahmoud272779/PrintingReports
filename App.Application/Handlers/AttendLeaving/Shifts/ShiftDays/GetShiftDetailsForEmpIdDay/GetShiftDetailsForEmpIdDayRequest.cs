using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace App.Application.Handlers.AttendLeaving.Shifts.ShiftDays.GetShiftDetailsForEmpIdDay
{
    public class GetShiftDetailsForEmpIdDayRequest :IRequest<ResponseResult>
    {
        [Required]
        public int EmpId { get; set;}
        [Required]
        public DateTime day { get; set;}

        public bool isEdited { get; set; } = false;

        public int id { get; set; } 
    }
}
