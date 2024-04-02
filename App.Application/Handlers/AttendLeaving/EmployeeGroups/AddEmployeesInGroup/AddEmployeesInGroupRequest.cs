using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.EmployeeGroups.AddEmployeesInGroup
{
    public class AddEmployeesInGroupRequest : App.Domain.Models.Request.AttendLeaving.AddEmployeesInParent, IRequest<ResponseResult>
    {
    }
}
