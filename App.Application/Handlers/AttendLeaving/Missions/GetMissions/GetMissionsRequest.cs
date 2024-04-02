using MediatR;

namespace App.Application.Handlers.AttendLeaving.Missions.GetMissions
{
    public class GetMissionsRequest : App.Domain.Models.Request.AttendLeaving.GetMissions, IRequest<ResponseResult>
    {
    }
}
