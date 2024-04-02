using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Application.Handlers.AttendLeaving.Missions.GetMissions;
using App.Domain.Entities.Process.AttendLeaving;
using MediatR;
using static App.Application.Handlers.AttendLeaving.Holidays.GetHolidays.GetHolidaysHandler;

namespace App.Application.Handlers.AttendLeaving.RamadanDates.GetRamadanDates
{
    public class GetRamadanDatesHandler : IRequestHandler<GetRamadanDatesRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.RamadanDate> _RamadanDatesQuery;

        public GetRamadanDatesHandler(IRepositoryQuery<RamadanDate> ramadanDatesQuery)
        {
            _RamadanDatesQuery = ramadanDatesQuery;
        }

        public async Task<ResponseResult> Handle(GetRamadanDatesRequest request, CancellationToken cancellationToken)
        {
            var data = _RamadanDatesQuery.TableNoTracking;
            var totalData = data.Count();
            var res = data.Where(c => !string.IsNullOrEmpty(request.searchCriteria) ? c.ArabicName.Contains(request.searchCriteria) || c.LatinName.Contains(request.searchCriteria) : true);
            var dataCount = res.Count();
            var response = res
                .Skip(((request.PageNumber ?? 0) - 1) * request.PageSize ?? 0)
                .Take(request.PageSize ?? 0)
                .ToList()
                 .Select(c => new GetHolidayDTO
                 {
                     Id = c.id,
                     arabicName = c.ArabicName,
                     latinName = c.LatinName,
                     startdate = c.FromDate,
                     enddate = c.ToDate,
                     duration = (c.ToDate - c.FromDate).Days,

                     statusEn = DateTime.Now < c.FromDate ? "Not started yet" : (DateTime.Now > c.FromDate && DateTime.Now < c.ToDate) ? "started" : "Ended",
                     statusAr = DateTime.Now < c.FromDate ? "لم تبدأ بعد" : (DateTime.Now > c.FromDate && DateTime.Now < c.ToDate) ? "سارية " : "انتهت",

                     status = DateTime.Now < c.FromDate ? StatusNumber.Not_started : (DateTime.Now > c.FromDate && DateTime.Now < c.ToDate) ? StatusNumber.StartingAndRunningNow : StatusNumber.Ended,
                     canDelete = true
                 });
            //.Select(c => new RamadanDatesResponseDTO
            //{
            //    Id = c.id,
            //    arabicName = c.ArabicName,
            //    latinName = c.LatinName,
            //    canDelete = true,
            //    dateFrom=c.FromDate, 
            //    dateTo=c.ToDate,
            //    note= c.Note??"",
            //});
            return new ResponseResult
            {
                Result = Result.Success,
                Data = response,
                Note = Aliases.GetEndOfData(request.PageSize ?? 0, dataCount, request.PageNumber ?? 0),
                DataCount = dataCount,
                TotalCount = totalData
            };

        }
    }
    public class RamadanDatesResponseDTO
    {
        public int Id { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
        public bool canDelete { get; set; }
        public string note { get; set; }
        public DateTime dateFrom { get; set; }
        public DateTime dateTo { get; set; }
    }
}
