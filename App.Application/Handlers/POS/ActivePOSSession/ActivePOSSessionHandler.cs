using App.Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.POS.ActivePOSSession
{
    public class ActivePOSSessionHandler : IRequestHandler<ActivePOSSessionRequest, ResponseResult>
    {
        private readonly IConfiguration _configuration;
        private readonly ClientSqlDbContext dbContext;

        public ActivePOSSessionHandler(IConfiguration configuration, ClientSqlDbContext dbContext)
        {
            _configuration = configuration;
            this.dbContext = dbContext;
        }

        public async Task<ResponseResult> Handle(ActivePOSSessionRequest request, CancellationToken cancellationToken)
        {
            var connectionString = $"Data Source={_configuration["ApplicationSetting:serverName"]};" +
                                       $"Initial Catalog={request.databaseName};" +
                                       $"user id={_configuration["ApplicationSetting:UID"]};" +
                                       $"password={_configuration["ApplicationSetting:Password"]};" +
                                       $"MultipleActiveResultSets=true;";

            dbContext.Database.SetConnectionString(connectionString);
            var employeeSession = dbContext.pOSSessions.Where(x => x.employeeId == request.employeeId && x.sessionStatus == (int)POSSessionStatus.bining).OrderBy(x => x.Id).LastOrDefault();
            if (employeeSession != null)
            {
                employeeSession.sessionStatus = (int)POSSessionStatus.active;
                dbContext.pOSSessions.Update(employeeSession);
                dbContext.SaveChanges();
            }
            return new ResponseResult() { Code = 200 };
        }
    }
}
