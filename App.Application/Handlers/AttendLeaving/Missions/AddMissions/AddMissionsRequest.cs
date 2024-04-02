using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Missions.AddMissions
{
    public class AddMissionsRequest : App.Domain.Models.Request.AttendLeaving.AddMissions,IRequest<ResponseResult>
    {
    }
}
