using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Application.Handlers.AttendLeaving.CalculatingWorkingHours.TimeCalculation;
using App.Application.Handlers.AttendLeaving.Reports.GetAbsanceReport;
using App.Application.Handlers.AttendLeaving.Reports.ReportHelper;
using App.Domain.Entities.Process.AttendLeaving;
using App.Domain.Entities.Process.AttendLeaving.Transactions;
using App.Domain.Models.Security.Authentication.Request;
using App.Infrastructure;
using MediatR;

namespace App.Application.Handlers.AttendLeaving.Reports.GetTotalLate
{
    public class GetTotalLateHandler : IRequestHandler<GetTotalLateRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<App.Domain.Entities.Process.InvEmployees> _employeesQuery;
        private readonly IRepositoryQuery<GLBranch> _GLBranchQuery;

        private readonly IRepositoryQuery<MoviedTransactions> _MoviedTransactionsQuery;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.HolidaysEmployees> _HolidaysEmployees;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.VaccationEmployees> _VaccationEmployeesQuery;
        private readonly IMediator _mediator;
        private readonly IRepositoryQuery<RamadanDate> _RamadanDateQuery;

        public GetTotalLateHandler(IRepositoryQuery<InvEmployees> employeesQuery, IRepositoryQuery<GLBranch> gLBranchQuery, IRepositoryQuery<MoviedTransactions> moviedTransactionsQuery, IRepositoryQuery<Domain.Entities.Process.AttendLeaving.HolidaysEmployees> holidaysEmployees, IRepositoryQuery<Domain.Entities.Process.AttendLeaving.VaccationEmployees> vaccationEmployeesQuery, IMediator mediator, IRepositoryQuery<RamadanDate> ramadanDateQuery)
        {
            _employeesQuery = employeesQuery;
            _GLBranchQuery = gLBranchQuery;
            _MoviedTransactionsQuery = moviedTransactionsQuery;
            _HolidaysEmployees = holidaysEmployees;
            _VaccationEmployeesQuery = vaccationEmployeesQuery;
            _mediator = mediator;
            _RamadanDateQuery = ramadanDateQuery;
        }


        public async Task<ResponseResult> Handle(GetTotalLateRequest request, CancellationToken cancellationToken)
        {
            var calcRes = await _mediator.Send(new TimeCalculationRequest());
            if (calcRes.Result != Result.Success)
                return calcRes;
            if (request.DateFrom > request.DateTo)
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = ErrorMessagesAr.StartDateAfterEndDate,
                        MessageEn = ErrorMessagesEn.StartDateAfterEndDate,
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };
            int[] branches = null;
            if (!string.IsNullOrEmpty(request.BranchId))
                branches = request.BranchId.Split(',').Select(c => int.Parse(c)).ToArray();

            var branchesObjs = _GLBranchQuery.TableNoTracking
                .Where(c => (branches != null && request.BranchId != "0" && branches.Any()) ? branches.Contains(c.Id) : true).ToList();

            var employees = _employeesQuery.TableNoTracking
                                .Include(c => c.shiftsMaster)
                                .ThenInclude(c => c.changefulTimeGroups)
                                .ThenInclude(c => c.changefulTimeDays)
                                .Include(c => c.shiftsMaster.normalShiftDetalies)
                                .Include(c => c.Job)
                                .Include(c => c.Sections)
                                .Include(c => c.Departments).Where(c => request.EmpId != null ? c.Id == request.EmpId : true)
                   .Where(c => request.DepartmentId != null ? c.DepartmentsId == request.DepartmentId :true)
                   .Where(c => request.SectionId != null ? c.SectionsId == request.SectionId:true)
                   .Where(c => (branches != null && branches.Any()) ? branches.Contains(c.gLBranchId) : true)
                   .Where(c => request.JobId != null ? c.JobId == request.JobId : true)
                   .Where(c => request.GroupId != null ? c.employeesGroupId == request.GroupId : true)
                   .Where(c => request.ShiftmasterId != null ? c.shiftsMasterId == request.ShiftmasterId : true)
                   .Where(c => request.projectId != null ? c.projectsId == request.projectId : true)
                   .ToList();
            var days = DatesService.GetDatesBetween(request.DateFrom, request.DateTo).ToList();

            var movedTransactions = _MoviedTransactionsQuery
                   .TableNoTracking
                   ?.Where(c => c.day >= request.DateFrom && c.day <= request.DateTo)
                   ?.Where(c => employees.Select(x => x.Id).ToArray().Contains(c.EmployeesId)).ToList();

            List<BranchDTOLate> ListOf_branches = new List<BranchDTOLate>();
           
                foreach (var branch in branchesObjs)
                {

                    BranchDTOLate thisBranch = new BranchDTOLate
                    {
                        BranchNameAr = branch.ArabicName,
                        BranchNameEn = branch.LatinName,
                        branchId = branch.Id,
                    };

                    thisBranch.Employees = new List<EmpDTO>();

                    var emps = employees.Where(c => c.gLBranchId == branch.Id);
                    foreach (var emp in emps)
                    {

                    if( GetTotalLateFunction.GetTotalLate(days, movedTransactions, emp, _HolidaysEmployees, _VaccationEmployeesQuery, _RamadanDateQuery, _mediator)!="00:00")
                            thisBranch.Employees.Add(new EmpDTO
                            {
                                totalLate = GetTotalLateFunction.GetTotalLate(days, movedTransactions, emp, _HolidaysEmployees, _VaccationEmployeesQuery, _RamadanDateQuery, _mediator),
                                branchId =branch.Id,
                                empId = emp.Id,
                                code=emp.Code,
                                empnameAr = emp.ArabicName,
                                empnameEn = emp.LatinName,
                                shiftmasterNameAr = emp.shiftsMaster?.arabicName,
                                shiftmasterNameEn = emp.shiftsMaster?.latinName,
                                JobNameAr = emp.Job?.ArabicName,
                                JobNameEn = emp.Job?.LatinName,


                            });
                        }




                if (thisBranch.Employees.Any())
                    ListOf_branches.Add(thisBranch);


            }
            return new ResponseResult
            {
                Result = Result.Success,
                Data = ListOf_branches
            };

        }

        


        
    }

    public class EmpDTO 
    {
        public int? empId { get; set; }
        public int? code { get; set; }
        public int? branchId { get; set; }
        public string? empnameAr { get; set; }
        public string? empnameEn { get; set; }

        public string? shiftmasterNameAr { get; set; }
        public string? shiftmasterNameEn { get; set; }

        public string? JobNameAr { get; set; }
        public string? JobNameEn { get; set; }

        public string? totalLate { get; set; }



    }


    public class BranchDTOLate
    {
        public string? BranchNameAr { get; set; }
        public string? BranchNameEn { get; set; }
        public int branchId { get; set; }
        public List<EmpDTO> Employees { get; set; }

    }
}
