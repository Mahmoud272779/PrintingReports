using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Shifts.ShiftMaster.EditMasterShift
{
    public class EditMasterShiftRequest : App.Domain.Models.Request.AttendLeaving.EditMasterShift,IRequest<ResponseResult>
    {
    }
}
