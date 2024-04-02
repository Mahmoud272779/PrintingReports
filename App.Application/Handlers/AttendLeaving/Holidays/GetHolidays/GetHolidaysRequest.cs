using MediatR;

namespace App.Application.Handlers.AttendLeaving.Holidays.GetHolidays
{
    public class GetHolidaysRequest : App.Domain.Models.Request.AttendLeaving.GetHolidays, IRequest<ResponseResult>
    {
    }
}
