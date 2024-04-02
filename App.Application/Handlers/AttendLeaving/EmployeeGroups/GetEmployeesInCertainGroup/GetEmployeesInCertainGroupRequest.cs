using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.EmployeeGroups.GetEmployeesInCertainGroup
{
    public class GetEmployeesInCertainGroupRequest :  App.Domain.Models.Request.AttendLeaving.GetEmployeesInCertainGroup, IRequest<ResponseResult>
    {
    }
}
