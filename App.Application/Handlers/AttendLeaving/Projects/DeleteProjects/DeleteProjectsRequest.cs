using MediatR;

namespace App.Application.Handlers.AttendLeaving.Projects.DeleteProjects
{
    public class DeleteProjectsRequest : App.Domain.Models.Request.AttendLeaving.DeleteMissions, IRequest<ResponseResult>
    {
    }
}
