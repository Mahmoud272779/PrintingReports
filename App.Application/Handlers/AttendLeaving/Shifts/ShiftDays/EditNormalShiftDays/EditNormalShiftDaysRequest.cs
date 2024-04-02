using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Shifts.EditNormalShiftDays
{
    public class EditNormalShiftDaysRequest : App.Domain.Models.Request.AttendLeaving.EditNormalShiftDaysDTO,IRequest<ResponseResult>
    {
    }
}
