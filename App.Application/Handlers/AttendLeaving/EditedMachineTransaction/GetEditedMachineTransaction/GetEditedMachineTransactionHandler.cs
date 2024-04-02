using App.Domain.Entities.Process.AttendLeaving.Transactions;
using App.Infrastructure.settings;
using MediatR;
using System.Threading;

namespace App.Application.Handlers.AttendLeaving.EditedMachineTransaction.GetEditedMachineTransaction
{
    public class GetEditedMachineTransactionHandler : IRequestHandler<GetEditedMachineTransactionRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<MachineTransactions> _MachineTransactionsQuery;
        private readonly IRepositoryQuery<InvEmployees> _InvEmployeesQuery;

        public GetEditedMachineTransactionHandler(IRepositoryQuery<MachineTransactions> machineTransactionsQuery, IRepositoryQuery<InvEmployees> invEmployeesQuery)
        {
            _MachineTransactionsQuery = machineTransactionsQuery;
            _InvEmployeesQuery = invEmployeesQuery;
        }

        public async Task<ResponseResult> Handle(GetEditedMachineTransactionRequest request, CancellationToken cancellationToken)
        {
            int[] branchIds = null;
            if (!string.IsNullOrEmpty(request.branchIds))
                branchIds = request.branchIds.Split(',').Select(c => int.Parse(c)).ToArray();
            int[] sectionId = null;
            if (!string.IsNullOrEmpty(request.sectionId))
                sectionId = request.sectionId.Split(',').Select(c => int.Parse(c)).ToArray();
            int[] departmentId = null;
            if (!string.IsNullOrEmpty(request.departmentId))
                departmentId = request.departmentId.Split(',').Select(c => int.Parse(c)).ToArray();

            var emps = _InvEmployeesQuery.TableNoTracking
                        .Include(c => c.Sections)
                        .Include(c => c.Departments)
                        .Where(c => request.empId != null ? (c.Code.ToString().Contains(request.empId) || c.ArabicName.Contains(request.empId) || c.LatinName.Contains(request.empId)) : true)
                        .Where(c => branchIds != null ? branchIds.Contains(c.gLBranchId) : true)
                        .Where(c => sectionId != null ? sectionId.Contains(c.SectionsId??0) : true)
                        .Where(c => departmentId != null ? departmentId.Contains(c.DepartmentsId??0) : true)
                        .Where(c => request.shiftId != null ? c.shiftsMasterId == request.shiftId : true);


            var _allData = _MachineTransactionsQuery
                                .TableNoTracking.Where(c => emps.Select(x => x.Code).ToArray().Contains(c.EmployeeCode))
                                .Where(c => !c.IsEdited ? (c.TransactionDate.Date >= request.dateFrom.Date && c.TransactionDate.Date <= request.dateTo.Date) : (c.EditedTransactionDate.Date >= request.dateFrom.Date && c.EditedTransactionDate.Date <= request.dateTo.Date));
            var totalCount = _allData.Count();
            var allData = _allData.OrderByDescending(c => c.Id);
            var dataCount = allData.Count();
            var res = allData.Skip(((request.PageNumber ?? 0) - 1) * (request.PageSize ?? 0))
                             .Take((request.PageSize ?? 0))
                             .ToList()
                             .Select(c => new EditedMachineTransactionResponseDTO
                             {
                                 Id = c.Id,
                                 code = c.EmployeeCode,
                                 machineDate = c.TransactionDate.ToString("yyyy-MM-ddTHH:mm"),
                                 editedDate = c.EditedTransactionDate.ToString("yyyy-MM-ddTHH:mm"),
                                 isEdited = c.IsEdited,
                                 isAuto = c.isAuto,
                                 machineId=c.machineId,
                                 employee = emps.Any(x => x.Code == c.EmployeeCode) ?
                                 new EditedMachineTransactionResponseDTO_Employee
                                 {
                                     Id = emps.FirstOrDefault(x => x.Code == c.EmployeeCode).Id,
                                     arabicName = emps.FirstOrDefault(x => x.Code == c.EmployeeCode).ArabicName,
                                     latinName = emps.FirstOrDefault(x => x.Code == c.EmployeeCode).LatinName,

                                 } : null
                             });

            return new ResponseResult()
            {
                Data = res,
                TotalCount = totalCount,
                DataCount = dataCount,
                Note = Aliases.GetEndOfData(request.PageSize ?? 0, totalCount, request.PageNumber ?? 0)
            };
        }
    }
    public class EditedMachineTransactionResponseDTO
    {
        public int Id { get; set; }
        public int machineId { get; set; }
        public int code { get; set; }
        public EditedMachineTransactionResponseDTO_Employee employee { get; set; }
        public string machineDate { get; set; }
        public string editedDate { get; set; }
        public bool isEdited { get; set; }
        public bool isAuto { get; set; }

    }
    public class EditedMachineTransactionResponseDTO_Employee
    {
        public int Id { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
    }
}
