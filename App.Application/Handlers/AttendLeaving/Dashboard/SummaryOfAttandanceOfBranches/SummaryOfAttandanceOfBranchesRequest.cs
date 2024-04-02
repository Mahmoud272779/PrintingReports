using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Dashboard.SummaryOfAttandanceOfBranches
{
    public class SummaryOfAttandanceOfBranchesRequest : IRequest<ResponseResult>
    {
    }
    public class SummaryOfAttandanceOfBranchesDetalies
    {
        public int Id { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
        public int AttendedCount { get; set; }
        public int absenceCount { get; set; }
        public int vacationsCount { get; set; }
        public int weekendCount { get; set; }
        public int waitingCount { get; set; }
    }
    public class SummaryOfAttandanceOfBranchesResponse
    {
        public List<SummaryOfAttandanceOfBranchesDetalies> detalies { get; set; }
        public double maxValue { get; set; } 
    }
}
