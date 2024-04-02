using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Vaccation.AddVaccation
{
    public class AddVaccationRequest : App.Domain.Models.Request.AttendLeaving.AddVaccatiuon, IRequest<ResponseResult>
    {
    }
}
