using System;
using Iprox.Application.TvShowFunc.Interfaces;
using Iprox.Application.TvShowFunc.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Iprox.Presentation.Functions.TvShowFunctions
{
    public class SyncData
    {
        private readonly ILogger _logger;
        private readonly ISyncDataService _syncDataService;

        public SyncData(ILoggerFactory loggerFactory, ISyncDataService syncDataService)
        {
            _logger = loggerFactory.CreateLogger<SyncData>();
            _syncDataService = syncDataService;
        }

        [Function("SyncData")]
        public async Task RunAsync([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");

            }

            bool isSuccess = await _syncDataService.SyncDataAsync();
            if (isSuccess)
            {
                _logger.LogInformation($"Data Sync Succesful");
            }
            else
            {
                _logger.LogCritical($"Data Sync Succesful");
            }
        }
    }
}
