using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace App.Application.Handlers.AttendLeaving.AttendanceMachines.AdvancedSearch_machines
{
    public class AdvancedSearch_machinesRequest : IRequest<ResponseResult>

    {
      public string? searchCriteria { set; get; }

    }
}
