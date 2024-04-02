using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Missions.DeleteMissions
{
    public class DeleteMissionsRequest : App.Domain.Models.Request.AttendLeaving.DeleteMissions, IRequest<ResponseResult>
    {
    }
}
