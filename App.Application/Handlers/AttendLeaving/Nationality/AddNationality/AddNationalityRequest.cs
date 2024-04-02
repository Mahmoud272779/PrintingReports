using MediatR;
using App.Domain.Models.Request.AttendLeaving;
namespace App.Application.Handlers.AttendLeaving.Nationality.AddNationality
{
    public class AddNationalityRequest : App.Domain.Models.Request.AttendLeaving.AddNationality, IRequest<ResponseResult>
    {
    }
}
