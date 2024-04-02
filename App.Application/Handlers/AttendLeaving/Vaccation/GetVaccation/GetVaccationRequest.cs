using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Vaccation.GetVaccation
{
    public class GetVaccationRequest : App.Domain.Models.Request.AttendLeaving.GetVacction, IRequest<ResponseResult>
    {
    }
}
