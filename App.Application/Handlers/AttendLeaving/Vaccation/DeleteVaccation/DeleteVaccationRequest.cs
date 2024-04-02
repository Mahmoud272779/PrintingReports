using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Vaccation.DeleteVaccation
{
    public class DeleteVaccationRequest : App.Domain.Models.Request.AttendLeaving.DeleteVaccation, IRequest<ResponseResult>
    
    {
    }
}
