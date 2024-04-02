using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Reports.GetAbsanceReport
{
    public class GetAbsanceReportRequest :App.Domain.Models.Request.AttendLeaving.Reports.GetAbsanceReportRequest, IRequest<ResponseResult>
    {

    }
}
