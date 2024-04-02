using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.VaccationEmployees.DeleteVaccationEmployees
{
    public class DeleteVaccationEmployeesRequest : App.Domain.Models.Request.AttendLeaving.DeleteVaccationEmployees, IRequest<ResponseResult>
    {
    }
}
