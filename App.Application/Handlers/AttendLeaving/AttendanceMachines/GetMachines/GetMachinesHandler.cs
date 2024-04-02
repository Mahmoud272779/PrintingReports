using App.Application.Services.Process.Invoices;
using App.Domain.Entities.Process.AttendLeaving;
using App.Domain.Entities.Process.AttendLeaving.Transactions;
using App.Infrastructure.settings;
using MediatR;
using Microsoft.AspNetCore.Server.HttpSys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.AttendanceMachines.GetMachines
{
    public class GetMachinesHandler : IRequestHandler<GetMachinesRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<Machines> _MachinesQuery;
        private readonly IRepositoryQuery<MachineTransactions> _MachineTransactionsQuery;

        public GetMachinesHandler(IRepositoryQuery<Machines> machinesQuery, IRepositoryQuery<MachineTransactions> machineTransactionsQuery)
        {
            _MachinesQuery = machinesQuery;
            _MachineTransactionsQuery = machineTransactionsQuery;
        }

        public async Task<ResponseResult> Handle(GetMachinesRequest request, CancellationToken cancellationToken)
        {
            int[] branches = null;
            if (!string.IsNullOrEmpty(request.branchId))
                branches = request.branchId.Split(',').Select(c => int.Parse(c)).ToArray();
            var machineTransactions = _MachineTransactionsQuery.TableNoTracking.OrderByDescending(c => c.PushTime);
            var data = _MachinesQuery
                        .TableNoTracking
                        .Include(c => c.branch);
            int totalCount = data.Count();
            var res = data
                .Where(c => !string.IsNullOrEmpty(request.searchCriteria) ? c.MachineSN.Contains(request.searchCriteria) || c.arabicName.Contains(request.searchCriteria) || c.latinName.Contains(request.searchCriteria) : true)
                .Where(c => branches != null ? branches.Contains(c.branchId) : true);
            var dataCount = res.Count();
            var response = res
                            .Skip(((request.PageNumber ?? 0) - 1) * (request.PageSize ?? 0))
                            .Take((request.PageSize ?? 0))
                            .ToList()
                            .Select(c => new GetMachinesResponseDTO
                            {
                                Id = c.Id,
                                machine = new machine
                                {

                                    arabicName = c.arabicName,
                                    latinName = c.latinName,
                                },
                                branch = new branch
                                {
                                    id = c.branchId,
                                    arabicName = c.branch.ArabicName,
                                    latinName = c.branch.LatinName
                                },
                                canDelete = true,
                                machineSN = c.MachineSN,

                                lastSeen = c.MachineTransactions?.OrderByDescending(p => p.PushTime).FirstOrDefault().PushTime.ToString(defultData.datetimeFormat) ?? null,
                                // lastSeen = machineTransactions.Any(x => x.machineId == c.Id) ? machineTransactions.FirstOrDefault(x=> x.machineId == c.Id).PushTime.ToString(defultData.datetimeFormat) : null,
                            });

            return new ResponseResult
            {
                Result = Result.Success,
                Data = response,
                DataCount = dataCount,
                TotalCount = totalCount,
                Note = Aliases.GetEndOfData(request.PageSize ?? 0, dataCount, request.PageNumber ?? 0),
            };
        }
    }
    public class GetMachinesResponseDTO
    {

        public int Id { get; set; }
        public branch branch { get; set; }
        public machine machine { get; set; }
        public bool canDelete { get; set; } = true;
        public string machineSN { get; set; }
        public string lastSeen { get; set; }
    }
    public class branch
    {
        public int id { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
    }

    public class machine
    {

        public string arabicName { get; set; }
        public string latinName { get; set; }
    }
}
