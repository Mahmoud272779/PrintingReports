using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Vaccation.EditVaccation
{
    public class EditVaccationRequest : App.Domain.Models.Request.AttendLeaving.EditVacction, IRequest<ResponseResult>
    {
    }
}
