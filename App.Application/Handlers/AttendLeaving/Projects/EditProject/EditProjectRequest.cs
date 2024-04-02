using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Projects.EditProject
{
    public class EditProjectRequest : App.Domain.Models.Request.AttendLeaving.EditMissions, IRequest<ResponseResult>
    {
    }
}
