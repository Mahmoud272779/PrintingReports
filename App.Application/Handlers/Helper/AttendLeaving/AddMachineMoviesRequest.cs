using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.Helper.AttendLeaving
{
    public class AddMachineMoviesRequest : IRequest<ResponseResult>
    {
        public int employeeCode { get; set; }
        public int CurrentEmployeeCode { get; set; }
    }
}
