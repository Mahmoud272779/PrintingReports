﻿using App.Domain.Models.Request.AttendLeaving;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Shifts.ShiftDays.ChangefulTime.ChangefulTime_GetEmps
{
    public class ChangefulTime_GetEmpsRequest : ChangefulTime_GetEmpsRequest_DTO,IRequest<ResponseResult>
    {
    }
}