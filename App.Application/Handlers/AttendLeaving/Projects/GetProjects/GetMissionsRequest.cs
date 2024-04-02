using MediatR;

namespace App.Application.Handlers.AttendLeaving.Missions.GetMissions
{
    public class GetProjectsRequest : App.Domain.Models.Request.AttendLeaving.GetPorject, IRequest<ResponseResult>
    {
    }
}
