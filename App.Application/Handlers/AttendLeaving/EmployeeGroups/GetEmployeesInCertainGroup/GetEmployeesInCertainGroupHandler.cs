using App.Application.Handlers.AttendLeaving.CalculatingWorkingHours.TimeCalculation.Models;
using App.Application.Handlers.AttendLeaving.EmployeeGroups.AddEmployeeInGroup;
using App.Application.Handlers.AttendLeaving.HolidaysEmployees.GetHolidaysEmployees;
using App.Application.Handlers.AttendLeaving.Missions.AddMissions;
using App.Infrastructure.UserManagementDB;
using DocumentFormat.OpenXml.Office2010.Excel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.EmployeeGroups.GetEmployeesInCertainGroup
{
    public class GetEmployeesInCertainGroupHandler : IRequestHandler<GetEmployeesInCertainGroupRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<InvEmployees> _InvEmployeesQuery;

        public GetEmployeesInCertainGroupHandler(IRepositoryQuery<InvEmployees> invEmployeesQuery)
        {
            _InvEmployeesQuery = invEmployeesQuery;
        }

        public async Task<ResponseResult> Handle(GetEmployeesInCertainGroupRequest request, CancellationToken cancellationToken)
        {
            var EmployeesInGroup = _InvEmployeesQuery.TableNoTracking
                .Include(c => c.GLBranch)
                .Include(c => c.shiftsMaster)
                .Where(c => c.employeesGroupId == request.groupId).ToList();


            var emps = EmployeesInGroup.Where(c => !string.IsNullOrEmpty(request.SearchCriteria) ?
           (request.SearchCriteria.Contains(c.Code.ToString()) || c.Code.ToString().Contains(request.SearchCriteria)
            || request.SearchCriteria.Contains(c.LatinName)
            || request.SearchCriteria.Contains(c.ArabicName)) : true);



            var dataCount = emps.Count();


            var response = emps
                .Skip(((request.PageNumber ?? 0) - 1) * request.PageSize ?? 0)
                .Take(request.PageSize ?? 0)
                .ToList()
                .Select(c => new GetEmpsInGroupDTO
                {

                    id = c.Id,
                    code = c.Code,
                    Employee = new Employeedto
                    {
                        arabicName = c.ArabicName,
                        latinName = c.LatinName,
                    },
                    Branch = new Branchdto
                    {
                        arabicName = c.GLBranch?.ArabicName ?? "",
                        latinName = c.GLBranch?.LatinName ?? "",
                    },
                    Shift = new Shiftdto
                    {
                        arabicName = c.shiftsMaster?.arabicName ?? "",
                        latinName = c.shiftsMaster?.latinName ?? "",
                    },
                    canDelete = true

                });

            return new ResponseResult
            {
                Result = Result.Success,
                Data = response,
                Note = Aliases.GetEndOfData(request.PageSize ?? 0, dataCount, request.PageNumber ?? 0),
                DataCount = dataCount,
                TotalCount = response.Count()
            };




        }


        public class GetEmpsInGroupDTO
        {
            public int id { get; set; }
            public int code { get; set; }
            public Employeedto Employee { get; set; }
            public Branchdto Branch { get; set; }
            public Shiftdto Shift { get; set; }
            public bool canDelete { get; set; }


        }
    }
}
