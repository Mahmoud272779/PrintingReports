using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.VaccationEmployees.GetVaccationEmployees
{
    public class GetVaccationEmployeesRequest : App.Domain.Models.Request.AttendLeaving.GetVaccationEmployees, IRequest<ResponseResult>
    {
    }
}
