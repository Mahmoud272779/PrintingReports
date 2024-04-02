using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Shifts.ShiftMaster.GetMasterShift
{
    public class GetMasterShiftRequest : App.Domain.Models.Request.AttendLeaving.GetMasterShift,IRequest<ResponseResult>
    {
    }
}
