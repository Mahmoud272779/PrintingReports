using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Shifts.ShiftDays.ChangefulTime.GetChangefulTimeDays
{
    public class GetChangefulTimeDaysRequest : App.Domain.Models.Request.AttendLeaving.GetChangefulTimeDays, IRequest<ResponseResult>
    {
    }
}
