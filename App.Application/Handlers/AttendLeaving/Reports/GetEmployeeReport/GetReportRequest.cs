﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Reports.GetReport
{
    public class GetReportRequest : App.Domain.Models.Request.AttendLeaving.Reports.GetReportRequest ,IRequest<ResponseResult>
    {
    }
}
