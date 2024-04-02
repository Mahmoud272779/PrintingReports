using DocumentFormat.OpenXml.Office2013.Excel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Dashboard.AttendingLeaveDetalies
{
    public class AttendingLeaveDetaliesRequest : IRequest<ResponseResult>
    {
        public int branchId { get; set; } = 0;
        public DateTime day { get; set; } = DateTime.Now;
    }
}
