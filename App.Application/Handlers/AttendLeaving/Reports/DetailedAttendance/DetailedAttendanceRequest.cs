using App.Domain.Models.Request.AttendLeaving.Reports;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Reports.DetailedAttendance
{
    public class DetailedAttendanceRequest : DetailedAttendanceRequestDTO,IRequest<ResponseResult>
    {
    }
}
