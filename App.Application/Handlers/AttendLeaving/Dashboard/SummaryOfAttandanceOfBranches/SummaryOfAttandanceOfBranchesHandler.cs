using App.Application.Handlers.AttendLeaving.Dashboard.AttendingLeaveDetalies;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Dashboard.SummaryOfAttandanceOfBranches
{
    public class SummaryOfAttandanceOfBranchesHandler : IRequestHandler<SummaryOfAttandanceOfBranchesRequest, ResponseResult>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryQuery<GLBranch> _GLBranchQuery;
        private readonly iUserInformation _iUserInformation;
        public SummaryOfAttandanceOfBranchesHandler(IMediator mediator, IRepositoryQuery<GLBranch> gLBranchQuery, iUserInformation iUserInformation)
        {
            _mediator = mediator;
            _GLBranchQuery = gLBranchQuery;
            _iUserInformation = iUserInformation;
        }

        public async Task<ResponseResult> Handle(SummaryOfAttandanceOfBranchesRequest request, CancellationToken cancellationToken)
        {
            var userInfo = await _iUserInformation.GetUserInformation();

            var branches = _GLBranchQuery.TableNoTracking.Where(c => userInfo.employeeBranches.Contains(c.Id));
            List<SummaryOfAttandanceOfBranchesDetalies> SummaryOfAttandanceOfBranchesDetalies = new List<SummaryOfAttandanceOfBranchesDetalies>();

            foreach (var branch in branches)
            {
                var data = (AttendingLeaveDetaliesResponse) _mediator.Send(new AttendingLeaveDetaliesRequest { branchId = branch.Id }).Result.Data;
                SummaryOfAttandanceOfBranchesDetalies.Add(new SummaryOfAttandanceOfBranches.SummaryOfAttandanceOfBranchesDetalies
                {
                    Id = branch.Id,
                    absenceCount = data.absenceCount,
                    AttendedCount = data.AttendedCount,
                    waitingCount = data.waitingCount,
                    vacationsCount = data.vacationsCount,
                    weekendCount = data.weekendCount,
                    arabicName = branch.ArabicName,
                    latinName = branch.LatinName
                });
            }
            SummaryOfAttandanceOfBranchesResponse summaryOfAttandanceOfBranchesResponse = new SummaryOfAttandanceOfBranchesResponse
            {
                 detalies = SummaryOfAttandanceOfBranchesDetalies,
                 maxValue = (SummaryOfAttandanceOfBranchesDetalies.Max(c=> c.absenceCount) + 
                 SummaryOfAttandanceOfBranchesDetalies.Max(c => c.AttendedCount) +
                 SummaryOfAttandanceOfBranchesDetalies.Max(c => c.waitingCount) +
                 SummaryOfAttandanceOfBranchesDetalies.Max(c => c.vacationsCount) +
                 SummaryOfAttandanceOfBranchesDetalies.Max(c => c.weekendCount) ) *1.1
            };
            return new ResponseResult
            {
                Result = Result.Success,
                Data = summaryOfAttandanceOfBranchesResponse
            };
        }
    }
}
