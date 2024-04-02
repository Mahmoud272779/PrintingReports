using App.Domain.Entities.Process.AttendLeaving;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.AttendanceMachines.machinesDropdownList
{
    public class machinesDropdownListHandler : IRequestHandler<machinesDropdownListRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<Machines> _MachinesQuery;
        private readonly iUserInformation _UserInformation;
        public machinesDropdownListHandler(IRepositoryQuery<Machines> machinesQuery, iUserInformation userInformation)
        {
            _MachinesQuery = machinesQuery;
            _UserInformation = userInformation;
        }

        public async Task<ResponseResult> Handle(machinesDropdownListRequest request, CancellationToken cancellationToken)
        {
            var userInfo = await _UserInformation.GetUserInformation();
            var data = _MachinesQuery.TableNoTracking.Where(c => userInfo.employeeBranches.Contains(c.branchId));
            var totalCount = data.Count();
            var filteredData = data.Where(c => !string.IsNullOrEmpty(request.searchCriteria) ? (request.searchCriteria.Contains(c.arabicName) || request.searchCriteria.Contains(c.latinName)) : true);
            var dataCount = filteredData.Count();
            var res = filteredData
                .Skip(((request.PageNumber ??0) - 1) * (request.PageSize ?? 0))
                .Take((request.PageSize ?? 0))
                .ToList()
                .Select(c => new dropdownListDTO
                {
                    Id = c.Id,
                    arabicName = c.arabicName,
                    latinName = c.latinName,
                });
            return new ResponseResult
            {
                Result = Result.Success,
                Data = res,
                TotalCount = totalCount,
                DataCount = dataCount
            };

        }
    }
    
}
