using App.Domain.Entities.Process.AttendLeaving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Helpers
{
    public static class CalcTimingHelper
    {
        public static bool IsRamadan(IRepositoryQuery<RamadanDate> _RamadanDateQuery, DateTime day)
        {
            bool isRamadan = _RamadanDateQuery.TableNoTracking.Any(c => day.Date >= c.FromDate.Date && day.Date <= c.ToDate.Date);
            return isRamadan;
        }
    }
}
