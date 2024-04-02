using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Application.Handlers.AttendLeaving.AttendLeaving_Helper;
using App.Application.Handlers.AttendLeaving.Reports.AttendLateLeaveEarly;
using App.Application.Handlers.AttendLeaving.Reports.ReportHelper;
using App.Domain.Entities.Process.AttendLeaving.Transactions;
using App.Domain.Models.Request.AttendLeaving.Reports;
using App.Infrastructure.settings;
using MediatR;

namespace App.Application.Handlers.AttendLeaving.Reports.AttendancePermissionsReport
{
    public class AttendancePermissionsReportHandler : IRequestHandler<AttendancePermissionsReportRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<App.Domain.Entities.Process.InvEmployees> _InvEmployeesQuery;
        private readonly IRepositoryQuery<GLBranch> _GLBranchQuery;

        public AttendancePermissionsReportHandler(IRepositoryQuery<InvEmployees> invEmployeesQuery, IRepositoryQuery<GLBranch> gLBranchQuery)
        {
            _InvEmployeesQuery = invEmployeesQuery;
            _GLBranchQuery = gLBranchQuery;
        }

        public async Task<ResponseResult> Handle(AttendancePermissionsReportRequest request, CancellationToken cancellationToken)
        {
            int[] branches = null;
            if (!string.IsNullOrEmpty(request.BranchId))
                branches = request.BranchId.Split(',').Select(c => int.Parse(c)).ToArray();
            var employees = _InvEmployeesQuery.TableNoTracking
                .Include(c => c.GLBranch)
                .Include(c => c.Job)
                .Include(c => c.shiftsMaster)
                .Include(c=>c.permissions)
                .Where(c => c.Status != (int)Status.newElement)
                .Where(c => request.EmpId != null ? c.Id == request.EmpId : true)
                .Where(c=>!string.IsNullOrEmpty( request.name)?c.ArabicName.Contains(request.name) || c.LatinName.Contains(request.name):true)
                .Where(c => c.DepartmentsId != null ? c.DepartmentsId == request.DepartmentId : true)
                .Where(c => c.SectionsId != null ? c.SectionsId == request.SectionId : true)
                .Where(c => (branches != null && branches.Any()) ? branches.Contains(c.gLBranchId) : true)
                .Where(c => request.JobId != null ? c.JobId == request.JobId : true)
                .Where(c => request.GroupId != null ? c.JobId == request.GroupId : true)
                .Where(c => request.ShiftmasterId != null ? c.shiftsMasterId == request.ShiftmasterId : true)
                .ToList();

            var days = DatesService.GetDatesBetween(request.DateFrom, request.DateTo).ToList();
            var branchesObjs = _GLBranchQuery.TableNoTracking
               .Where(c => (branches != null && request.BranchId != "0" && branches.Any()) ? branches.Contains(c.Id) : true).ToList();


            List<AttendancePermisionsReportResponseDTO_Branches> ListOf_branches = new List<AttendancePermisionsReportResponseDTO_Branches>();

            foreach (var branch in branchesObjs)
            {
                AttendancePermisionsReportResponseDTO_Branches thisBranch = new AttendancePermisionsReportResponseDTO_Branches
                {
                    Id = branch.Id,
                    arabicName = branch.ArabicName,
                    latinName = branch.LatinName
                };
                thisBranch.employees = new List<AttendancePermisionsReportResponseDTO_Employees>();
                var emps = employees.Where(c => c.gLBranchId == branch.Id);
                

                foreach (var emp in emps)
                {
                    
                    
                    List<AttendancePermisionsReportResponseDTO_Days> ListOf_days = new List<AttendancePermisionsReportResponseDTO_Days>();



                    foreach (var day in days)
                    {
                     
                        if (!emp.permissions.Any(c => c.Day.Date == day.Date))
                            continue;

                        var permission = emp.permissions.FirstOrDefault(c => request.permissionType != null && request.permissionType != 0? c.Day.Date == day.Date && c.type == request.permissionType : c.Day.Date == day.Date);
                       


                        List<AttendancePermisionsReportResponseDTO_Permissions> ListOf_permissions = new List<AttendancePermisionsReportResponseDTO_Permissions>();
                       
                       
                            
                            if (permission.type == (int)Enums.PermissionTypeEnum.Day)
                            {
                                ListOf_permissions.Add(new AttendancePermisionsReportResponseDTO_Permissions
                                {
                                    permissionType = "يومى",
                                    permissionTypeEn="Day",
                                    permissionShiftEn="",
                                    dayId=Lists.days.Where(c => c.latinName == day.DayOfWeek.ToString()).FirstOrDefault().Id,
                                    branchId = branch.Id,
                                    employeeId = emp.Id,

                                    note = permission.note??"",

                                    time_In =  defultData.EmptyAttendance,
                                    time_Out = defultData.EmptyAttendance,

                                });
                                
                            }

                            

                            else
                            {
                              
                                    ListOf_permissions.Add(new AttendancePermisionsReportResponseDTO_Permissions
                                    {
                                        permissionType =  "مؤقت",
                                        dayId = Lists.days.Where(c => c.latinName == day.DayOfWeek.ToString()).FirstOrDefault().Id,
                                        branchId = branch.Id,
                                        employeeId = emp.Id,
                                        permissionTypeEn = "Temp",
                                        permissionShiftEn = "First",
                                        time_In = permission.shift1_start?.ToString("HH:mm") ?? defultData.EmptyAttendance,
                                        time_Out = permission.shift1_end?.ToString("HH:mm") ?? defultData.EmptyAttendance,
                                        permissionShift = "الاولى",
                                        note = permission.note ,

                                    });
                                   

                                if (permission.haveShift2)
                                {
                                    ListOf_permissions.Add(new AttendancePermisionsReportResponseDTO_Permissions
                                    {
                                        permissionType = "مؤقت",
                                        dayId = Lists.days.Where(c => c.latinName == day.DayOfWeek.ToString()).FirstOrDefault().Id,
                                        branchId = branch.Id,
                                        employeeId = emp.Id,
                                        time_In = permission.shift2_start?.ToString("HH:mm") ?? defultData.EmptyAttendance,
                                        time_Out = permission.shift2_end?.ToString("HH:mm") ?? defultData.EmptyAttendance,
                                        permissionShift = "الثانية",
                                        note = permission.note,
                                        permissionTypeEn = "Temp",
                                        permissionShiftEn = "Second",


                                    });
                                }

                                if (permission.haveShift3)
                                {
                                    ListOf_permissions.Add(new AttendancePermisionsReportResponseDTO_Permissions
                                    {
                                        permissionType = "مؤقت",
                                        dayId = Lists.days.Where(c => c.latinName == day.DayOfWeek.ToString()).FirstOrDefault().Id,
                                        branchId = branch.Id,
                                        employeeId = emp.Id,
                                        time_In = permission.shift3_start?.ToString("HH:mm") ?? defultData.EmptyAttendance,
                                        time_Out = permission.shift3_end?.ToString("HH:mm") ?? defultData.EmptyAttendance,
                                        permissionShift = "الثالثة",
                                        note = permission.note,
                                        permissionTypeEn = "Temp",
                                        permissionShiftEn = "Third",

                                    });
                                }

                                if (permission.haveShift4)
                                {
                                    ListOf_permissions.Add(new AttendancePermisionsReportResponseDTO_Permissions
                                    {
                                        permissionType = "مؤقت",
                                        dayId = Lists.days.Where(c => c.latinName == day.DayOfWeek.ToString()).FirstOrDefault().Id,
                                        branchId = branch.Id,
                                        employeeId = emp.Id,
                                        time_In = permission.shift4_start?.ToString("HH:mm") ?? defultData.EmptyAttendance,
                                        time_Out = permission.shift4_end?.ToString("HH:mm") ?? defultData.EmptyAttendance,
                                        permissionShift = "الرابعة",
                                        note = permission.note,
                                        permissionTypeEn = "Temp",
                                        permissionShiftEn = "Fourth",

                                    });
                                }
                            }


                        ListOf_days.Add(new AttendancePermisionsReportResponseDTO_Days
                        {
                            Id = Lists.days.Where(c => c.latinName == day.DayOfWeek.ToString()).FirstOrDefault().Id,
                            branchId = branch.Id,
                            employeeId = emp.Id,
                            date = day.ToString("yyyy-MM-dd") ?? "",
                            dayAr = Lists.days.Where(c => c.latinName == day.DayOfWeek.ToString()).FirstOrDefault().arabicName,
                            dayEn = Lists.days.Where(c => c.latinName == day.DayOfWeek.ToString()).FirstOrDefault().latinName,
                            permissions = ListOf_permissions,



                        }) ;






                    }
                    if(ListOf_days.Count!=0)
                        thisBranch.employees.Add(new AttendancePermisionsReportResponseDTO_Employees
                        {
                            code = emp.Code,
                            arabicName = emp.ArabicName??"",
                            latinName = emp.LatinName??"",
                            jobAr = emp.Job != null ? emp.Job?.ArabicName : defultData.EmptyAttendance,
                            jobEn = emp.Job != null ? emp.Job?.LatinName : defultData.EmptyAttendance,

                            Id = emp.Id,
                            branchId = emp.gLBranchId,
                            shiftAr = emp.shiftsMaster?.arabicName ?? defultData.EmptyAttendance,
                            shiftEn = emp.shiftsMaster?.latinName ?? defultData.EmptyAttendance,
                            days = ListOf_days
                        });
                }
                if (thisBranch.employees.Any())
                    ListOf_branches.Add(thisBranch);

            }

            return new ResponseResult
            {
                Result = Result.Success,
                Data = ListOf_branches
            };


        }
    }


    public class AttendancePermisionsReportResponseDTO_Days
    {
        public int? branchId { get; set; }
        public int? employeeId { get; set; }
        public int? Id { get; set; }
        public string? dayAr { get; set; }
        public string? dayEn { get; set; }
        public string? date { get; set; }
        
        public List<AttendancePermisionsReportResponseDTO_Permissions> permissions { get; set; }
    }

    public class AttendancePermisionsReportResponseDTO_Permissions 
    {

        public string? permissionType { get; set; }
        public string? permissionShift { get; set; }
        public string? permissionTypeEn { get; set; }
        public string? permissionShiftEn { get; set; }
        public int? branchId { get; set; }
        public int? employeeId { get; set; }
        public int? dayId { get; set; }
        public string? time_In { get; set; }
        public string? time_Out { get; set; }
        public string? note { get; set; }


    }
    public class AttendancePermisionsReportResponseDTO_Employees
    {
        public int? Id { get; set; }
        public int? branchId { get; set; }
        public int? code { get; set; }
        public string? arabicName { get; set; }
        public string? latinName { get; set; }

        public string? shiftAr { get; set; }
        public string? shiftEn { get; set; }

        public string? jobAr { get; set; }
        public string? jobEn { get; set; }
        
        

        public List<AttendancePermisionsReportResponseDTO_Days> days { get; set; }
    }
    public class AttendancePermisionsReportResponseDTO_Branches
    {
        public int? Id { get; set; }
        
        public string? arabicName { get; set; }
        public string? latinName { get; set; }
        public List<AttendancePermisionsReportResponseDTO_Employees> employees { get; set; }

    }
}
