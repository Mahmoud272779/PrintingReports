using App.Domain.Entities.Process.AttendLeaving;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Nationality.GetNationality
{
    public class GetNationalityHandler : IRequestHandler<GetNationalityRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.Nationality> _NationalityQuery;

        public GetNationalityHandler(IRepositoryQuery<Domain.Entities.Process.AttendLeaving.Nationality> nationalityQuery)
        {
            _NationalityQuery = nationalityQuery;
        }

        public async Task<ResponseResult> Handle(GetNationalityRequest request, CancellationToken cancellationToken)
        {
            var data = _NationalityQuery.TableNoTracking.Include(c => c.InvEmployees);
            var TotalCount = data.Count();
            var res = data.Where(c => !string.IsNullOrEmpty(request.searchCriteria) ? c.arabicName.Contains(request.searchCriteria) || c.latinName.Contains(request.searchCriteria) : true);
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
                    canDelete = !c.InvEmployees.Any()
                });
            return new ResponseResult
            {
                Data = response.OrderByDescending(c => c.Id),
                Result = Result.Success
            };
        }
    }
    public class NationalityResponseDTO
    {
        public int Id { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
        public bool canDelete { get; set; }
    }
}
