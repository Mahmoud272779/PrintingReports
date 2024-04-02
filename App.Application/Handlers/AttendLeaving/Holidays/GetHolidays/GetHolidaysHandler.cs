using MediatR;
using System.Threading;

namespace App.Application.Handlers.AttendLeaving.Holidays.GetHolidays
{
    public class GetHolidaysHandler : IRequestHandler<GetHolidaysRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.Holidays> _holidaysQuery;

        public GetHolidaysHandler(IRepositoryQuery<Domain.Entities.Process.AttendLeaving.Holidays> holidaysQuery)
        {
            _holidaysQuery = holidaysQuery;
        }

        public async Task<ResponseResult> Handle(GetHolidaysRequest request, CancellationToken cancellationToken)
        {

            var data = _holidaysQuery.TableNoTracking.Include(c => c.EmployeesHolidays).ToList();

            var totalData = data.Count();
            var res = data;
            var dataCount = res.Count();


            var response = res
                .Skip(((request.PageNumber ?? 0) - 1) * request.PageSize ?? 0)
                .Take(request.PageSize ?? 0)
                .Where(c => !string.IsNullOrEmpty(request.SearchCriteria) ? c.arabicName.Contains(request.SearchCriteria) || c.latinName.Contains(request.SearchCriteria) : true)
                .ToList()
                .Select(c => new GetHolidayDTO
                {
                    Id = c.Id,
                    arabicName = c.arabicName,
                    latinName = c.latinName,
                    startdate = c.startdate,
                    enddate = c.enddate,
                    duration = (c.enddate - c.startdate).Days,

                    statusEn = DateTime.Now < c.startdate ? "Not started yet" : (DateTime.Now > c.startdate && DateTime.Now < c.enddate) ? "started" : "Ended",
                    statusAr = DateTime.Now < c.startdate ? "لم تبدأ بعد" : (DateTime.Now > c.startdate && DateTime.Now < c.enddate) ? "سارية " : "انتهت",

                    status = DateTime.Now < c.startdate ? StatusNumber.Not_started : (DateTime.Now > c.startdate && DateTime.Now < c.enddate) ? StatusNumber.StartingAndRunningNow : StatusNumber.Ended,
                    canDelete = !c.EmployeesHolidays.Any()
                });
            return new ResponseResult
            {
                Result = Result.Success,
                Data = response.OrderByDescending(c => c.Id),
                Note = Aliases.GetEndOfData(request.PageSize ?? 0, dataCount, request.PageNumber ?? 0),
                DataCount = dataCount,
                TotalCount = totalData
            };
        }

        public StatusResult Getstatus(DateTime startdate , DateTime enddate ) 
        {
            string statusar = "";
            string statusen = "";
            StatusNumber statusnumber;
            if (DateTime.Now < startdate)
            {
                statusar = "لم تبدأ بعد";
                statusen = "Not started";
                statusnumber = StatusNumber.Not_started;
            }
            else if (DateTime.Now > startdate && DateTime.Now < enddate)
            {
                statusar = "سارية";
                statusen = "started";
                statusnumber = StatusNumber.StartingAndRunningNow;
            }
            else 
            {
                statusar = "انتهت";
                statusen = "Ended";
                statusnumber = StatusNumber.Ended;
            }

            return new StatusResult
            { statusAr = statusar, statusEn = statusen, statusNumber = statusnumber };
        }

        public class StatusResult
        {
            public string statusAr { get; set; }
            public string statusEn { get; set; }
            public StatusNumber statusNumber { get; set; }
        }

        public enum StatusNumber
        {
            Not_started = 0,
            StartingAndRunningNow = 1,
            Ended = 2
        }
        public class GetHolidayDTO
        {
            public int Id { get; set; }
            public string arabicName { get; set; }
            public string latinName { get; set; }

            public DateTime startdate { get; set; }
            public DateTime enddate { get; set; }

            public double duration { get; set; }

            public string statusAr { get; set; }

            public string statusEn { get; set; }

            public bool canDelete { get; set; }
            public StatusNumber status { get; set; }
        }
    }
}
