using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Reports.GetTotalAbsanceReport
{
   public class GetTotalAbsanceReportRequest : App.Domain.Models.Request.AttendLeaving.Reports.GetTotalAbsanceReportRequest, IRequest<ResponseResult>
    {
    }
}
