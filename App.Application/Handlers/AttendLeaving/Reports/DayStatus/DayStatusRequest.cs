using App.Domain.Models.Request.AttendLeaving.Reports;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Reports.DayStatus
{
    public class DayStatusRequest : DayStatues,IRequest<ResponseResult>
    {
    }
}
