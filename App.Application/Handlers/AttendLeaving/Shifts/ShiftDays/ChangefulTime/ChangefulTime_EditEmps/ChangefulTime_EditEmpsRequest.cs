using App.Domain.Models.Request.AttendLeaving;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Shifts.ShiftDays.ChangefulTime.ChangefulTime_AddEmps
{
    public class ChangefulTime_EditEmpsRequest : ChangefulTime_Employees_EditDTO, IRequest<ResponseResult>
    {
    }
}
