using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Shifts.ShiftMaster.DeleteMasterShift
{
    public class DeleteMasterShiftRequest : App.Domain.Models.Request.AttendLeaving.DeleteMasterShift,IRequest<ResponseResult>
    {
    }
}
