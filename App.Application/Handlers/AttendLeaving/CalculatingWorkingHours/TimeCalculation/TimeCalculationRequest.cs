using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.CalculatingWorkingHours.TimeCalculation
{
    public class TimeCalculationRequest : IRequest<ResponseResult>
    {
        public int[] employeesCode { get; set; }
    }
}
