using App.Application.Handlers.AttendLeaving.HolidaysEmployees.GetHolidaysEmployees;
using App.Domain.Models.Response.HR.AttendLeaving;
using FastReport.Utils;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.EmployeeGroups.AddEmployeeInGroup
{
    public class GetEmployeesForAddModelHandler : IRequestHandler<GetEmployeesForAddModelRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<InvEmployees> _InvEmployeesQuery;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.EmployeesGroup> _EmployeesGroupQuery;
        public GetEmployeesForAddModelHandler(IRepositoryQuery<InvEmployees> invEmployeesQuery, IRepositoryQuery<Domain.Entities.Process.AttendLeaving.EmployeesGroup> employeesGroupQuery)
        {
            _InvEmployeesQuery = invEmployeesQuery;
            _EmployeesGroupQuery = employeesGroupQuery;
        }



        //public string? name { get; set; }
        
        //public int? SectionsId { get; set; }


        //[Required]

        //public int groupId { get; set; }
        


        


        public async Task<ResponseResult> Handle(GetEmployeesForAddModelRequest request, CancellationToken cancellationToken)
        {
            int[] branches = null;

            if (!string.IsNullOrEmpty(request.branchId) && request.branchId != "0")
                branches = request.branchId.Split(',').Select(c => int.Parse(c)).ToArray();
            var EmployeesNotInGroup = _InvEmployeesQuery.TableNoTracking
                .Include(c=>c.GLBranch)
                .Include(c=>c.shiftsMaster)
                .Where(c => c.employeesGroupId==null).ToList();


            var emps = EmployeesNotInGroup

              .Where(c => request.code != null && request.code != 0 ? c.Code.ToString().Contains(request.code.ToString()) : true)
               .Where(c => branches != null ? branches.Contains(c.gLBranchId) : true)
               .Where(c=> request.name!=null && c.LatinName!=null ? c.ArabicName.Contains(request.name) || c.LatinName.Contains(request.name) :true)
               .Where(c => request.SectionsId != null && request.SectionsId != 0 ? c.SectionsId == request.SectionsId : true)
               .Where(c => request.DepartmentId != null && request.DepartmentId != 0 ? c.DepartmentsId == request.DepartmentId : true)
               .Where(c => request.ShiftId != null && request.ShiftId != 0 ? c.shiftsMasterId == request.ShiftId : true)
               .Where(c => request.JobId != null && request.JobId != 0 ? c.JobId == request.JobId : true);


            var dataCount = emps.Count();


            var response = emps
                .Skip(((request.PageNumber ?? 0) - 1) * request.PageSize ?? 0)
                .Take(request.PageSize ?? 0)
                .ToList()
                .Select(c => new GetDTO
                {
                    code=c.Code,
                    id=c.Id,
                   arabicName=c.ArabicName,
                   latinName = c.LatinName

                });

            return new ResponseResult
            {
                Result = Result.Success,
                Data = response,
                Note = Aliases.GetEndOfData(request.PageSize ?? 0, dataCount, request.PageNumber ?? 0),
                DataCount = dataCount,
                TotalCount = dataCount
            };

        }
    }

    public class GetDTO {
        public int code { get; set; }
        public int id { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
    }
}
