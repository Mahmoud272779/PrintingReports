using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.PushDataAPI
{
    public class PushDataAPIRequest : IRequest<bool>
    {
        public int employeeCode { get; set; }
        public string name { get; set; }
        public DateTime transactionDate { get;set; }
        public string MahcineSN { get; set; }
        public string SecKey { get; set; }
        public string databaseName { get; set; }
        public string companyLogin { get; set; }
    }
}
