using App.Domain.Entities.Process.AttendLeaving.Transactions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.AttendLeaving_Helper
{
    public class AttendLeaving_GetStatusRequest : IRequest<AttendLeavingStatus>
    {
        public DateTime day { get; set; }
        public InvEmployees employees { get; set; }
        public MoviedTransactions transactions { get; set; }
    }
}
