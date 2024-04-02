using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Missions.AddMissions
{
    public class AddEmployeesGroupRequest : App.Domain.Models.Request.AttendLeaving.AddEmployeeGroups, IRequest<ResponseResult>
    {
    }
}
