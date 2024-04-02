using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Shifts.ShiftMaster.AddMasterShift
{
    public class AddMasterShiftRequest : App.Domain.Models.Request.AttendLeaving.AddMasterShift,IRequest<ResponseResult>
    {
    }
}
