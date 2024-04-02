using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Application.Handlers.AttendLeaving.Nationality.GetNationality;
using App.Domain.Entities.Process.AttendLeaving;
using MediatR;

namespace App.Application.Handlers.AttendLeaving.Religion.GetAll
{
    public class GetAllHandler : IRequestHandler<GetAllRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.religions> _religionsQuery;

        public GetAllHandler(IRepositoryQuery<religions> religionsQuery)
        {
            _religionsQuery = religionsQuery;
        }

        public async Task<ResponseResult> Handle(GetAllRequest request, CancellationToken cancellationToken)
        {
            var data = _religionsQuery.TableNoTracking.Include(c=>c.Employees);
            var TotalCount = data.Count();
            var res = data.Where(c => !string.IsNullOrEmpty(request.searchCriteria) ? request.searchCriteria.Contains(c.arabicName) || request.searchCriteria.Contains(c.latinName) : true);
            var dataCount = res.Count();
            var response = res
                .Skip(((request.PageNumber ?? 0) - 1) * (request.PageSize ?? 0))
                .Take(request.PageSize ?? 0)
                .ToList()
                .Select(c => new NationalityResponseDTO
                {
                    Id = c.Id,
                    arabicName = c.arabicName,
                    latinName = c.latinName,
                    canDelete = !c.Employees.Any()
                });
            return new ResponseResult
            {
                Data = response,
                Result = Result.Success
            };
        }
    }
}
