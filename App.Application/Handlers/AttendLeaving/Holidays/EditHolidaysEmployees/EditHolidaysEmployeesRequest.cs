using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Holidays.EditHolidaysEmployees
{
    public class EditHolidaysEmployeesRequest : App.Domain.Models.Request.AttendLeaving.EditHolidaysEmployees , IRequest<ResponseResult>
    {
    }
}
