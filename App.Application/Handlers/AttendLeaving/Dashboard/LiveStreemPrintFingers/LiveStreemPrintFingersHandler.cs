using App.Domain.Entities.Process.AttendLeaving.Transactions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Dashboard.LiveStreemPrintFingers
{
    public class LiveStreemPrintFingersHandler : IRequestHandler<LiveStreemPrintFingersRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<InvEmployees> _invEmployeesQuery;
        private readonly IRepositoryQuery<MachineTransactions> _MachineTransactionsQuery;

        public LiveStreemPrintFingersHandler(IRepositoryQuery<InvEmployees> invEmployeesQuery, IRepositoryQuery<MachineTransactions> machineTransactionsQuery)
        {
            _invEmployeesQuery = invEmployeesQuery;
            _MachineTransactionsQuery = machineTransactionsQuery;
        }

        public async Task<ResponseResult> Handle(LiveStreemPrintFingersRequest request, CancellationToken cancellationToken)
        {
            var employees = _invEmployeesQuery.TableNoTracking;
            var transactions = _MachineTransactionsQuery
                .TableNoTracking
                .OrderByDescending(c => c.TransactionDate)
                .Take(10)
                .Include(c=> c.machine)
                .ThenInclude(c=> c.branch)
                .ToList()
                .Select(c => new listStreemPrintFingersResponse
                {
                    Id = c.Id,
                    code = c.EmployeeCode,
                    arabicName = employees.FirstOrDefault(x=> x.Code == c.EmployeeCode)?.ArabicName??"",
                    latinName = employees.FirstOrDefault(x=> x.Code == c.EmployeeCode)?.LatinName??"",
                    location = new listStreemPrintFingersResponse_Branch
                    {
                        Id = c.machine.branchId,
                        arabicName = c.machine.branch.ArabicName,
                        latinName = c.machine.branch.LatinName,
                    }
                });
            return new ResponseResult
            {
                Result = Result.Success,
                Data = transactions
            };
        }
    }
    public class listStreemPrintFingersResponse
    {
        public int Id { get; set; }
        public int code { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
        public listStreemPrintFingersResponse_Branch location { get; set; }

    }
    public class listStreemPrintFingersResponse_Branch
    {
        public int Id { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
    }
}
