using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.SectionsAndDepartments.GetSectionsAndDepartments
{
    public class GetSectionsAndDepartmentsHandler : IRequestHandler<GetSectionsAndDepartmentsRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.SectionsAndDepartments> _sectionsAndDepartmentsQuery;
        private readonly IRepositoryQuery<InvEmployees> _InvEmployeesQuery;

        public GetSectionsAndDepartmentsHandler(IRepositoryQuery<Domain.Entities.Process.AttendLeaving.SectionsAndDepartments> sectionsAndDepartmentsQuery, IRepositoryQuery<InvEmployees> invEmployeesQuery)
        {
            _sectionsAndDepartmentsQuery = sectionsAndDepartmentsQuery;
            _InvEmployeesQuery = invEmployeesQuery;
        }

        public GetSectionsAndDepartmentsHandler()
        {
        }

        public async Task<ResponseResult> Handle(GetSectionsAndDepartmentsRequest request, CancellationToken cancellationToken)
        {
            var allData = _sectionsAndDepartmentsQuery.TableNoTracking
                .Include(c => c.emp)
                .Include(c => c.invEmployeesSection)
                .Include(c => c.invEmployeesDepartment);
            var data = allData.Where(c => c.Type == (int)request.Type && c.parentId == request.ParentId);
            var totalCount = data.Count();
            var res = data.Where(c => !string.IsNullOrEmpty(request.searchCriteria) ? (request.searchCriteria.Contains(c.arabicName) || request.searchCriteria.Contains(c.latinName)) : true).ToHashSet();
            var dataCount = res.Count();
            var response = res.Skip(((request.PageNumber ?? 0) - 1) * request.PageSize ?? 0).Take(request.PageSize ?? 0)
                .Select(c => new App.Domain.Models.Response.HR.AttendLeaving.GetSectionsAndDepartments
                {
                    Id = c.Id,
                    arabicName = c.arabicName,
                    latinName = c.latinName,
                    manager = c.empId != null ?new Domain.Models.Response.HR.AttendLeaving.Manager { Id = c.empId.Value,ArabicName = c.emp.ArabicName,LatinName = c.emp.LatinName }:null,
                    ManagerAr = c.emp != null ?c.emp.ArabicName :"",
                    ManagerEn = c.emp != null ?c.emp.LatinName :"",
                    employeeCount = request.Type == SectionsAndDepartmentsType.Sections ? c.invEmployeesSection.Count() : c.invEmployeesDepartment.Count(),
                    sectionsCount = allData.Where(v => v.Type == (int)Enums.SectionsAndDepartmentsType.Sections && v.parentId == c.Id).Count(),
                    CanDelete = c.Type == (int)SectionsAndDepartmentsType.Sections ? (c.invEmployeesSection != null ? true : false) : false
                });
            return new ResponseResult
            {
                Result = Result.Success,
                Data = response,
                DataCount = dataCount,
                TotalCount = totalCount
            };

        }
    }
}
