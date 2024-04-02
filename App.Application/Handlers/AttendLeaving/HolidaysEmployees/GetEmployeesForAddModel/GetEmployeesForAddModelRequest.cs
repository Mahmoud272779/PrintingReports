using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.HolidaysEmployees.GetEmployeesForAddModel
{
    public class GetEmployeesForAddModelRequest : App.Domain.Models.Request.AttendLeaving.GetEmployeesForAddModelDTO,IRequest<ResponseResult>
    {
    }
}
