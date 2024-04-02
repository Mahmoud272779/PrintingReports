using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.EmployeeGroups.DeleteEmployeesFromGroup
{
    public class DeleteEmployeesFromGroupRequest : App.Domain.Models.Request.AttendLeaving.DeleteEmployeesFromCertainGroup, IRequest<ResponseResult>
    {
    }
}
