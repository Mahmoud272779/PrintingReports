using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Shifts.ShiftDays.GetNormalShiftDays
{
    public class GetNormalShiftDaysRequest : App.Domain.Models.Request.AttendLeaving.GetNormalShiftDaysRequest, IRequest<ResponseResult>
    {
    }
}
