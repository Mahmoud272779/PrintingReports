﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.AttendancePermission.EditAttendancePermission
{
    public class EditAttendancePermissionRequest :App.Domain.Models.Request.AttendLeaving.EditAttendancePermission , IRequest<ResponseResult>
    {
    }
}
