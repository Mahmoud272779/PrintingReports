using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.AttendancePermission.GetAttendancePermission
{
    public class GetAttendancePermissionRequest : App.Domain.Models.Request.AttendLeaving.GetAttendancePermission, IRequest<ResponseResult>
    {
    }
}
