using MediatR;

namespace App.Application.Handlers.AttendLeaving.Missions.GetMissions
{
    public class GetEmployeeGroupsRequest : App.Domain.Models.Request.AttendLeaving.GetEmployeeGroups, IRequest<ResponseResult>
    {
    }
}
