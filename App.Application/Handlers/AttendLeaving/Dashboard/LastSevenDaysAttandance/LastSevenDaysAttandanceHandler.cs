using App.Application.Handlers.AttendLeaving.Dashboard.AttendingLeaveDetalies;
using App.Infrastructure.settings;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Dashboard.LastSevenDaysAttandance
{
    public class LastSevenDaysAttandanceHandler : IRequestHandler<LastSevenDaysAttandanceRequest, ResponseResult>
    {
        private readonly IMediator _mediator;

        public LastSevenDaysAttandanceHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ResponseResult> Handle(LastSevenDaysAttandanceRequest request, CancellationToken cancellationToken)
        {
            var days = DatesService.GetDatesBetween(DateTime.Now.Date.AddDays(-7), DateTime.Now.Date.AddDays(-1));
            List<LastSevenDaysAttendanceDetalies> detalies = new List<LastSevenDaysAttendanceDetalies>();
            foreach (var item in days)
            {
                var data = (AttendingLeaveDetaliesResponse)_mediator.Send(new AttendingLeaveDetaliesRequest {day = item }).Result.Data;
                detalies.Add(new LastSevenDaysAttendanceDetalies
                {
                    day = item.Date.ToString(defultData.datetimeFormat),
                    AttendedCount = data.AttendedCount,
                    absenceCount = data.absenceCount,
                    vacationsCount = data.vacationsCount,
                    waitingCount = data.waitingCount,
                    weekendCount = data.weekendCount
                });
            }
            var response = new LastSevenDaysAttendanceResponse
            {
                detalies = detalies,
                maxValue = (
                detalies.Max(c=> c.absenceCount +
                detalies.Max(c => c.AttendedCount) +
                detalies.Max(c => c.vacationsCount) +
                detalies.Max(c => c.waitingCount) +
                detalies.Max(c => c.weekendCount))
                ) * 1.1
            };
            return new ResponseResult
            {
                Result = Result.Success,
                Data = response
            };

        }
    }
    public class LastSevenDaysAttendanceDetalies
    {
        public int AttendedCount { get; set; }
        public int absenceCount { get; set; }
        public int vacationsCount { get; set; }
        public int weekendCount { get; set; }
        public int waitingCount { get; set; }
        public string day { get; set; }
    }
    public class LastSevenDaysAttendanceResponse
    {
        public List<LastSevenDaysAttendanceDetalies> detalies { get; set; }
        public double maxValue { get; set; }

    }
}
