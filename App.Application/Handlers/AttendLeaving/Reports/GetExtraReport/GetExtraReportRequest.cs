using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Reports.GetExtraReport
{
    public class GetExtraReportRequest : App.Domain.Models.Request.AttendLeaving.Reports.GetExtraReportRequest, IRequest<ResponseResult>
    {
    }
}
