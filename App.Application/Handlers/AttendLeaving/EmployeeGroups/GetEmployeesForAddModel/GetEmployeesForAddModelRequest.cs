using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.EmployeeGroups.AddEmployeeInGroup
{
    public class GetEmployeesForAddModelRequest : App.Domain.Models.Request.AttendLeaving.GetEmpsForAddModel, IRequest<ResponseResult>
    {
    }
}
