using App.Application.Handlers.AttendLeaving.Reports.GetReport;
using App.Domain.Entities.Process.AttendLeaving.Transactions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Reports.GetExtraReport
{
    public class GetExtraReportHandler : IRequestHandler<GetExtraReportRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.Transactions.MoviedTransactions> _movedTransaction;

        public GetExtraReportHandler(IRepositoryQuery<MoviedTransactions> movedTransaction)
        {
            _movedTransaction = movedTransaction;
        }

        public async Task<ResponseResult> Handle(GetExtraReportRequest request, CancellationToken cancellationToken)
        {
            int[] branches = null;
            if (!string.IsNullOrEmpty(request.BranchId))
                branches = request.BranchId.Split(',').Select(c => int.Parse(c)).ToArray();
            var data = _movedTransaction.TableNoTracking
                .Include(c => c.Employees)
                .ThenInclude(c =>  c.Sections)
                .Include(c => c.Employees.Departments)
                .Where(c => c.Employees.Status != (int)Status.newElement)
                .Where(c => request.EmpId != null ? c.EmployeesId == request.EmpId : true)
                .Where(c => request.DepartmentId != null ? c.Employees.DepartmentsId == request.DepartmentId : true)
                .Where(c => request.SectionId != null ? c.Employees.SectionsId == request.SectionId : true)
                .Where(c => (branches != null && branches.Any()) ? branches.Contains(c.Employees.gLBranchId) : true)
                .Where(c => request.JobId != null ? c.Employees.JobId == request.JobId : true)
                .Where(c => request.GroupId != null ? c.Employees.JobId == request.GroupId : true)
                .Where(c => request.ShiftmasterId != null ? c.Employees.shiftsMasterId == request.ShiftmasterId : true)
                .Where(d=>d.day>request.DateFrom && d.day<request.DateTo)
                .GroupBy(d => d.day)
                .ToList();


            var totalData = data.Count();
            var res = data;
            var dataCount = res.Count();



            var response = res

                .Select(c => new EmpDTO
                {
                   
                    //EmpId = c.Id,
                    //EmpNameAr = c?.ArabicName,
                    //EmpNameEn = c?.LatinName,
                    //BranchNameAr = c.GLBranch?.ArabicName,
                    //BranchNameEn = c.GLBranch?.LatinName,
                    //SectionNameAr = c.SectionsAndDepartments?.Type == (int)SectionsAndDepartmentsType.Sections ? c.SectionsAndDepartments?.arabicName : "",
                    //SectionNameEn = c.SectionsAndDepartments?.Type == (int)SectionsAndDepartmentsType.Sections ? c.SectionsAndDepartments?.latinName : "",
                    //DepNameAr = c.SectionsAndDepartments?.Type == (int)SectionsAndDepartmentsType.Departments ? c.SectionsAndDepartments?.arabicName : "",
                    //DepNameEn = c.SectionsAndDepartments?.Type == (int)SectionsAndDepartmentsType.Departments ? c.SectionsAndDepartments?.latinName : "",
                    //JobNameAr = c.Job?.ArabicName,
                    //JobNameEn = c.Job?.LatinName,
                    //GroupNameAr = c.employeesGroup?.arabicName,
                    //GroupNameEn = c.employeesGroup?.latinName,
                    //shiftmasterNameAr = c.shiftsMaster?.arabicName,
                    //shiftmasterNameEn = c.shiftsMaster?.latinName
                });
            return new ResponseResult
            {
                Result = Result.Success,
                Data = response,

                DataCount = dataCount,
                TotalCount = totalData
            };
        }

        
    }
    public class ExtraDTO 
    {
        public int MyProper { get; set; }
    }
}
