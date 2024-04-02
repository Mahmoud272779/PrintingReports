using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Domain.Models.Request.AttendLeaving.Reports;
using MediatR;

namespace App.Application.Handlers.AttendLeaving.Reports.DetailedDailyLate
{
    public class DetailedDailyLateRequest : GetDetailedLateReportRequestDTO, IRequest<ResponseResult>
    {

    }
}
