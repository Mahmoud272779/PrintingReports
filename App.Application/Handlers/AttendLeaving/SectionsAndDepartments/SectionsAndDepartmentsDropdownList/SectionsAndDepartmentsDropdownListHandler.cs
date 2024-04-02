using App.Domain.Models.Response.HR.AttendLeaving;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.SectionsAndDepartments.SectionsAndDepartmentsDropdownList
{
    public class SectionsAndDepartmentsDropdownListHandler : IRequestHandler<SectionsAndDepartmentsDropdownListRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.SectionsAndDepartments> _SectionsAndDepartmentsQuery;

        public SectionsAndDepartmentsDropdownListHandler(IRepositoryQuery<Domain.Entities.Process.AttendLeaving.SectionsAndDepartments> sectionsAndDepartmentsQuery)
        {
            _SectionsAndDepartmentsQuery = sectionsAndDepartmentsQuery;
        }

        public async Task<ResponseResult> Handle(SectionsAndDepartmentsDropdownListRequest request, CancellationToken cancellationToken)
        {
            int[] parentIds = request.parentId.Split(',').Select(c => int.Parse(c)).ToArray();
            var data = _SectionsAndDepartmentsQuery.TableNoTracking
                .Where(c => request.parentId != "0" ? parentIds.Contains(c.parentId) : true)
                .Where(c => c.Type == (int)request.Type)
                .ToHashSet()
                 .Select(c => new GetSectionsAndDepartmentsDropDownList
                 {
                     Id = c.Id,
                     arabicName = c.arabicName,
                     latinName = c.latinName
                 });
            var totalCount = data.Count();
                         
            return new ResponseResult
            {
                DataCount = totalCount,
                TotalCount = totalCount,
                Result = Result.Success,
                Data = data,
            };
        }
    }
}
