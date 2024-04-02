using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Vaccation.GetVaccationDropDownList
{
    public class GetVaccationDropDownRequestHandler : IRequestHandler<GetVaccationDropDownRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.Vaccation> _VaccationQuery;

        public GetVaccationDropDownRequestHandler(IRepositoryQuery<Domain.Entities.Process.AttendLeaving.Vaccation> vaccationQuery)
        {
            _VaccationQuery = vaccationQuery;
        }

        public async Task<ResponseResult> Handle(GetVaccationDropDownRequest request, CancellationToken cancellationToken)
        {
            var data = _VaccationQuery
                .TableNoTracking
                .Where(c => !string.IsNullOrEmpty(request.SearchCriteria) ? (request.SearchCriteria.Contains(c.ArabicName) || request.SearchCriteria.Contains(c.LatinName)) : true)
                .Select(c => new VaccationDropDownResponseDTO
                {
                    Id = c.Id,
                    arabicName = c.ArabicName,
                    latinName = c.LatinName
                });
            return new ResponseResult
            {
                Data = data,
                Result = Result.Success
            };
        }
    }
    public class VaccationDropDownResponseDTO
    {
        public int Id { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }

    }
}
