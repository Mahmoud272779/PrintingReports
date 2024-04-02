using App.Application.Handlers.AttendLeaving.Reports.GetAbsanceReport;
using App.Application.Handlers.AttendLeaving.Reports.ReportHelper;
using App.Domain.Entities.Process.AttendLeaving.Transactions;
using App.Infrastructure.settings;
using App.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Domain.Models.Security.Authentication.Request;
using App.Infrastructure.UserManagementDB;
using Org.BouncyCastle.Ocsp;
using App.Application.Handlers.AttendLeaving.GettingCompanyData.GettingCompanyDataDTOS;

namespace App.Application.Handlers.AttendLeaving.Reports.GetTotalAbsanceReport
{


    public class GetTotalAbsanceReportHandler : IRequestHandler<GetTotalAbsanceReportRequest, ResponseResult>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.InvEmployees> _employeesQuery;

        public GetTotalAbsanceReportHandler(IRepositoryQuery<InvEmployees> employeesQuery, IRepositoryQuery<GLBranch> gLBranchQuery, IRepositoryQuery<MoviedTransactions> moviedTransactionsQuery, IRepositoryQuery<Domain.Entities.Process.AttendLeaving.HolidaysEmployees> holidaysEmployees, IRepositoryQuery<Domain.Entities.Process.AttendLeaving.VaccationEmployees> vaccationEmployeesQuery, IMediator mediator)
        {
            _employeesQuery = employeesQuery;
            _GLBranchQuery = gLBranchQuery;
            _MoviedTransactionsQuery = moviedTransactionsQuery;
            _HolidaysEmployees = holidaysEmployees;
            _VaccationEmployeesQuery = vaccationEmployeesQuery;
            _mediator = mediator;
        }

        private readonly IRepositoryQuery<GLBranch> _GLBranchQuery;

        private readonly IRepositoryQuery<MoviedTransactions> _MoviedTransactionsQuery;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.HolidaysEmployees> _HolidaysEmployees;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.VaccationEmployees> _VaccationEmployeesQuery;
        public async Task<ResponseResult> Handle(GetTotalAbsanceReportRequest request, CancellationToken cancellationToken)
        {
            var data = await _mediator.Send(new GetAbsanceReportRequest
            {
                dateFrom = request.dateFrom,
                dateTo = request.dateTo,
                GroupId = request.GroupId,
                SectionId = request.SectionId,
                ShiftmasterId = request.ShiftmasterId,
                BranchId = request.BranchId,
                DepartmentId = request.DepartmentId,
                EmpId = request.EmpId,
                JobId = request.JobId,
            });
            if (data.Result != Result.Success)
                return data;
            List<DayDTO> response = (List<DayDTO>)data.Data;


            var result = response
                .SelectMany(day => day.Branches)
                .SelectMany(branch => branch.Employees, (branch, employee) => new
                {
                    Employeecode = employee.empcode,
                    EmployeeId = employee.empId,
                    BranchId = branch.branchId,
                    code = employee.empcode,
                    jobAr = employee.jobAr,
                    jobEn = employee.jobEn,
                    BranchNameAr = branch.BranchNameAr,
                    BranchNameEn = branch.BranchNameEn,
                    shiftmasterAr = employee.shiftmasterNameAr,
                    shiftmasterEn = employee.shiftmasterNameEn,
                    EmployeeNameAr = employee.empnameAr,
                    EmployeeNameEn = employee.empnameEn,

                })
                .GroupBy(resultItem => new { resultItem.BranchId, resultItem.EmployeeId })
                .Select(group => new
                {
                    BranchId = group.Key.BranchId,
                    EmployeeId = group.Key.EmployeeId,
                    code = group.First().code,
                    jobAr = group.First().jobAr,
                    jobEn = group.First().jobEn,
                    BranchNameAr = group.First().BranchNameAr,
                    BranchNameEn = group.First().BranchNameEn,
                    ShiftmasterAr = group.First().shiftmasterAr,
                    ShiftmasterEn = group.First().shiftmasterEn,
                    EmployeeNameAr = group.First().EmployeeNameAr,
                    EmployeeNameEn = group.First().EmployeeNameEn,
                    TotalAbsenceDays = group.Count()
                })
                .GroupBy(resultItem => resultItem.BranchId, resultItem => new TotalAbsance_Employees
                {
                    Id = resultItem.EmployeeId.Value,
                    code = resultItem.code ?? 0,
                    BranchId = resultItem.BranchId,
                    BranchNameAr = resultItem.BranchNameAr,
                    BranchNameEn = resultItem.BranchNameEn,
                    ShiftAr = resultItem.ShiftmasterAr,
                    ShiftEn = resultItem.ShiftmasterEn,
                    jobAr = resultItem.jobAr,
                    jobEn = resultItem.jobEn,
                    arabicName = resultItem.EmployeeNameAr,
                    latinName = resultItem.EmployeeNameEn,
                    TotalAbsenceDays = resultItem.TotalAbsenceDays
                })
                .Select(group => new TotalAbsance_Branches
                {

                    Id = group.Key,
                    arabicName = group.First().BranchNameAr,
                    latinName = group.First().BranchNameEn,
                    employees = group.ToList()
                })
                .ToList();





            return new ResponseResult
            {
                DataCount = result.Count(),
                Result = Result.Success,
                Data = result

            };



        }
    }

    public class TotalAbsance_Branches
    {
        public int Id { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
        public List<TotalAbsance_Employees> employees { get; set; }
    }
    public class TotalAbsance_Employees
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public int code { get; set; }
        public string BranchNameAr { get; set; }
        public string BranchNameEn { get; set; }
        public string jobAr { get; set; }
        public string jobEn { get; set; }
        public string ShiftAr { get; set; }
        public string ShiftEn { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
        public int TotalAbsenceDays { get; set; }
    }



    public class EmployeeWithTotalAbsanceDaysDTO
    {
        public int? empId { get; set; }
        public string? empnameAr { get; set; }
        public string? empnameEn { get; set; }
        public int? totalabsancedays { get; set; }
        public string? SectionNameAr { get; set; }
        public string? SectionNameEn { get; set; }

        public string? DepNameAr { get; set; }
        public string? DepNameEn { get; set; }

        public string? ProjectNameAr { get; set; }
        public string? ProjectNameEn { get; set; }

        public string? BranchNameAr { get; set; }
        public string? BranchNameEn { get; set; }

        public string? shiftmasterNameAr { get; set; }
        public string? shiftmasterNameEn { get; set; }

    }
    public class BranchWithTotalAbsanceDaysDTO
    {
        public string? BranchNameAr { get; set; }
        public string? BranchNameEn { get; set; }
        public int branchId { get; set; }
        public List<EmployeeWithTotalAbsanceDaysDTO> Employees { get; set; }
    }

    public class DayWithTotalAbsanceDTO
    {
        public string dayAr { get; set; }
        public string dayEn { get; set; }
        public string date { get; set; }
        public List<BranchWithTotalAbsanceDaysDTO> Branches { get; set; }
    }
}
