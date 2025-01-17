﻿using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Common.Behaviours
{
    public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : MediatR.IRequest<TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;
        //private readonly ICurrentUserService _currentUserService;
        //private readonly IIdentityService _identityService;

        //public PerformanceBehaviour(
        //    ILogger<TRequest> logger,
        //    ICurrentUserService currentUserService,
        //    IIdentityService identityService)
        //{
        //    _timer = new Stopwatch();

        //    _logger = logger;
        //    _currentUserService = currentUserService;
        //    _identityService = identityService;
        //}
        public PerformanceBehaviour(
           ILogger<TRequest> logger)
        {
            _timer = new Stopwatch();

            _logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();

            TResponse response = await next();

            _timer.Stop();

            long elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds > 500)
            {
                //var requestName = typeof(TRequest).Name;
                //var userId = _currentUserService.UserId ?? string.Empty;
                //var userName = string.Empty;

                //if (!string.IsNullOrEmpty(userId))
                //{
                //    userName = await _identityService.GetUserNameAsync(userId);
                //}

                //_logger.LogWarning("New_folder Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@UserName} {@Request}",
                //    requestName, elapsedMilliseconds, userId, userName, request);
            }

            return response;
        }
    }

}
