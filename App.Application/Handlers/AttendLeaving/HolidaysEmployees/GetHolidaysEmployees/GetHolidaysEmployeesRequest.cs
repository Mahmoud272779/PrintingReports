using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.HolidaysEmployees.GetHolidaysEmployees
{
    public class GetHolidaysEmployeesRequest : App.Domain.Models.Request.AttendLeaving.GetHolidaysEmployees, IRequest<ResponseResult>
    {
    }
}
