using App.Domain.Models.Request.AttendLeaving;
using MediatR;

namespace App.Application.Handlers.AttendLeaving.Reports.TotalAttendance
{
    public class TotalAttendanceRequest : TotalAttendanceRequestDTO,IRequest<ResponseResult>
    {
    }
}
