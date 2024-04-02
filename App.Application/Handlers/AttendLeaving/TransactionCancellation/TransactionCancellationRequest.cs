using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.TransactionCancellation
{
    public class TransactionCancellationRequest : IRequest<ResponseResult>
    {
        public DateTime dateFrom { get; set; }
        public DateTime dateTo { get; set; }
        public string? empIds { get; set; }
        public int? shiftIds { get; set; }
        public int? branchId { get; set; }
        public int? sectionId { get; set; }
        public int? departmentId { get; set; }
    }
}
