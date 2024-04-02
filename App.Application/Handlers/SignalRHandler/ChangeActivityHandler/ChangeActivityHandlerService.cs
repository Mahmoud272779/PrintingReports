using App.Domain.Entities.Process.General;
using App.Infrastructure;
using App.Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.SignalRHandler.ChangeActivityHandler
{
    public class ChangeActivityHandlerService : IRequestHandler<ChangeActivityHandlerRequest, ResponseResult>
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ClientSqlDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public ChangeActivityHandlerService(ClientSqlDbContext dbContext, Microsoft.Extensions.Configuration.IConfiguration configuration, IMemoryCache memoryCache)
        {

            _dbContext = dbContext;
            _configuration = configuration;
            _memoryCache = memoryCache;
        }

        public async Task<ResponseResult> Handle(ChangeActivityHandlerRequest request, CancellationToken cancellationToken)
        {
            var _cashHelper = new MemoryCashHelper(_memoryCache);
            var userSignalRInfo = _cashHelper.GetSignalRCashedValues().Where(x=> x.connectionId == request.connectionId).FirstOrDefault();
            if (userSignalRInfo == null)
                return new ResponseResult();
            var connectionString = $"Data Source={_configuration["ApplicationSetting:serverName"]};" +
                                       $"Initial Catalog={userSignalRInfo.DBName};" +
                                       $"user id={_configuration["ApplicationSetting:UID"]};" +
                                       $"password={_configuration["ApplicationSetting:Password"]};" +
                                       $"MultipleActiveResultSets=true;";
            _dbContext.Database.SetConnectionString(connectionString);
            var findSignalREmp = _dbContext.signalR.Where(x => x.InvEmployeesId == userSignalRInfo.EmployeeId).FirstOrDefault();
            if (findSignalREmp != null)
            {
                findSignalREmp.isOnline = request.isActive;
                findSignalREmp.connectionId = request.connectionId;
                _dbContext.signalR.Update(findSignalREmp);
                await _dbContext.SaveChangesAsync();

            }
            else
            {
                _dbContext.signalR.Add(new signalR()
                {
                    connectionId = request.connectionId,
                    isOnline = request.isActive,
                    InvEmployeesId = userSignalRInfo.EmployeeId
                });
                await _dbContext.SaveChangesAsync();
            }
            return new ResponseResult();
        }
    }
}
