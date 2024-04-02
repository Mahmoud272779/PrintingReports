using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.VaccationEmployees.AddVaccationEmployees
{
    public class AddVaccationEmployeesRequest : App.Domain.Models.Request.AttendLeaving.AddVaccationEmployees, IRequest<ResponseResult>
    {
    }
}
