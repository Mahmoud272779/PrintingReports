using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Missions.DeleteMissions
{
    public class DeleteEmployeeGroupsRequest : App.Domain.Models.Request.AttendLeaving.DeleteEmployeeGroups, IRequest<ResponseResult>
    {
    }
}
