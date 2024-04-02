using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.HolidaysEmployees.DeleteHolidaysEmployees
{
    public class DeleteHolidaysEmployeesRequest : App.Domain.Models.Request.AttendLeaving.DeleteHolidaysEmployees, IRequest<ResponseResult>
    {

    }
}
