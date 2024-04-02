using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.AttendancePermission.DeleteAttendancePermission
{
    public class DeleteAttendancePermissionRequest : App.Domain.Models.Request.AttendLeaving.DeleteAttendancePermission, IRequest<ResponseResult>
    {
    }
}
