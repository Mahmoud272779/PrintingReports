using App.Domain.Models.Response.HR.AttendLeaving.Reports;
using MediatR;
using Microsoft.AspNetCore.Server.HttpSys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Reports.vecationsReport
{
    public class vecationsReportHandler : IRequestHandler<vecationsReportRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.VaccationEmployees> _VaccationEmployeesQuery;
        private readonly IRepositoryQuery<GLBranch> _GLBranchQuery;

        public vecationsReportHandler(IRepositoryQuery<Domain.Entities.Process.AttendLeaving.VaccationEmployees> vaccationEmployeesQuery, IRepositoryQuery<GLBranch> gLBranchQuery)
        {
            _VaccationEmployeesQuery = vaccationEmployeesQuery;
            _GLBranchQuery = gLBranchQuery;
        }
        public async Task<ResponseResult> Handle(vecationsReportRequest request, CancellationToken cancellationToken)
        {
            int[] branches = null;
            int[] managements = null;
            int[] sections = null;
            if (!string.IsNullOrEmpty(request.branches))
                branches = request.branches.Split(',').Select(c => int.Parse(c)).ToArray();
            if (!string.IsNullOrEmpty(request.managements))
                managements = request.managements.Split(',').Select(c => int.Parse(c)).ToArray();
            if (!string.IsNullOrEmpty(request.sections))
                sections = request.sections.Split(',').Select(c => int.Parse(c)).ToArray();

            var data = _VaccationEmployeesQuery
                .TableNoTracking
                .Include(c => c.Vaccations)
                .Include(c => c.Employees)
                .Include(c => c.Employees.shiftsMaster)
                .Include(c => c.Employees.Job)
                .Where(c => request.employeeId != null ? c.EmployeeId == request.employeeId : true)
                .Where(c => branches != null ? c.EmployeeId == request.employeeId : true)
                .Where(c => managements != null ? c.EmployeeId == request.employeeId : true)
                .Where(c => sections != null ? c.EmployeeId == request.employeeId : true)
                .Where(c => request.projectId != null ? c.Employees.projectsId == request.projectId : true)
                .Where(c => request.jobId != null ? c.Employees.JobId == request.jobId : true)
                .Where(c => request.groupId != null ? c.Employees.employeesGroupId == request.groupId : true)
                .Where(c => request.shiftId != null ? c.Employees.shiftsMasterId == request.shiftId : true)
                .Where(c => request.vecationTypeId != null ? c.VaccationId == request.vecationTypeId : true)
                .Where(c => c.DateFrom.Date >= request.dateFrom.Date && c.DateTo.Date <= request.dateTo.Date);
            var AllBranches = _GLBranchQuery.TableNoTracking.Include(c => c.Employees).Where(c => c.Employees.Any());
            List<vecationsReportResponseDTO> response = new List<vecationsReportResponseDTO>();
            foreach (var branch in AllBranches)
            {
                var employees = data
                    .Where(c => c.Employees.gLBranchId == branch.Id)
                    .ToList()
                    .GroupBy(c => c.EmployeeId)
                    .Select(c => new vecationsReport_Employees
                    {
                        Id = c.First().EmployeeId,
                        BranchId = branch.Id,
                        code = c.First().Employees.Code,
                        arabicName = c.First().Employees.ArabicName,
                        latinName = c.First().Employees.LatinName,
                        jobAr = c.First().Employees.Job != null ? c.First().Employees.Job.ArabicName : "",
                        jobEn = c.First().Employees.Job != null ? c.First().Employees.Job.LatinName : "",
                        shiftAr = c.First().Employees.shiftsMaster != null ? c.First().Employees.shiftsMaster.arabicName : "",
                        shiftEn = c.First().Employees.shiftsMaster != null ? c.First().Employees.shiftsMaster.latinName : "",
                        job = c.First().Employees.Job != null ? new Employee_job
                        {
                            Id = c.First().Employees.Job.Id,
                            arabicName = c.First().Employees.Job.ArabicName,
                            latinName = c.First().Employees.Job.LatinName
                        } : null,
                        shift = c.First().Employees.shiftsMaster != null ? new Employee_shift
                        {
                            Id = c.First().Employees.shiftsMaster.Id,
                            arabicName = c.First().Employees.shiftsMaster.arabicName,
                            latinName = c.First().Employees.shiftsMaster.latinName
                        } : null,
                        totalDays = c.Sum(c => (c.DateTo - c.DateFrom).TotalDays + 1),
                        days = c.Select(x => new vecationsReport_days
                        {
                            Id = x.VaccationId,
                            BranchId = branch.Id,
                            EmployeeId = c.First().Employees.Id,
                            arabicName = x.Vaccations.ArabicName,
                            latinName = x.Vaccations.LatinName,
                            dateFrom = x.DateFrom.ToString("yyyy-MM-dd"),
                            dateTo = x.DateTo.ToString("yyyy-MM-dd"),
                            note = x.Note,
                            totalDays = (x.DateTo.Date - x.DateFrom.Date).TotalDays + 1
                        }).ToList()

                    })
                    .ToList();

                response.Add(new vecationsReportResponseDTO
                {
                    Id = branch.Id,
                    arabicName = branch.ArabicName,
                    latinName = branch.LatinName,
                    employees = employees
                });
            }
            return new ResponseResult
            {
                Data = response,
                Result = Result.Success
            };

        }
    }
}
