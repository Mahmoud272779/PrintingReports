using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.AttendancePermission.AddAttendancePermission
{
    public class AddAttendancePermissionRequset : App.Domain.Models.Request.AttendLeaving.AddAttendancePermission, IRequest<ResponseResult>
    {
        public bool isUpdate { get; set; }
        public int Id { get; set; }
    }
}
