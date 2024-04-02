using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Shifts.ShiftDays.ChangefulTime.EditChangefulTimeDays
{
    public  class EditChangefulTimeDaysRequest : App.Domain.Models.Request.AttendLeaving.EditChangefulTimeDays,IRequest<ResponseResult>
    {
    }
}
