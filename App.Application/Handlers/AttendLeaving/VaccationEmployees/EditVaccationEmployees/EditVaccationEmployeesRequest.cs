using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.VaccationEmployees.EditVaccationEmployees
{
    public class EditVaccationEmployeesRequest : App.Domain.Models.Request.AttendLeaving.EditVaccationEmployees, IRequest<ResponseResult>
    {
    }
}
