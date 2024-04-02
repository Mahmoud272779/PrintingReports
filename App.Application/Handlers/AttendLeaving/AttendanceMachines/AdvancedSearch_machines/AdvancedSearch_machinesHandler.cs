using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Application.Handlers.AttendLeaving.AttendanceMachines.GetMachines;
using App.Domain.Entities.Process.AttendLeaving;
using MediatR;

namespace App.Application.Handlers.AttendLeaving.AttendanceMachines.AdvancedSearch_machines
{
    public class AdvancedSearch_machinesHandler : IRequestHandler<AdvancedSearch_machinesRequest, ResponseResult>
    {

        private readonly IRepositoryQuery<Machines> _MachinesQuery;

        public AdvancedSearch_machinesHandler(IRepositoryQuery<Machines> machinesQuery)
        {
            _MachinesQuery = machinesQuery;
        }

        public async Task<ResponseResult> Handle(AdvancedSearch_machinesRequest request, CancellationToken cancellationToken)
        {
            var allMachines = _MachinesQuery.TableNoTracking.Include(c=>c.branch);
            int totalCount=allMachines.Count();

            var machinesBranch=allMachines.Where(c=>!string.IsNullOrEmpty (request.searchCriteria) ? c.branch.ArabicName.Contains(request.searchCriteria) || c.branch.LatinName.Contains(request.searchCriteria) : true).ToList();
            var data = machinesBranch.Select(c => new 
            {
                id=c.Id,
                arabicName = c.arabicName,
                latinName = c.latinName,
            });

            return new ResponseResult
            {
                Result=Result.Success,
                Data = machinesBranch,
                TotalCount = totalCount,
                DataCount= machinesBranch.Count(),


            };
        }
    }
}
