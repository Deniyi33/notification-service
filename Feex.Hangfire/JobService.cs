using Hangfire;
using Hangfire.Storage;
using Microsoft.Extensions.Options;
using Feex.Application.Interface;
using Feex.Application.Services;
using Feex.Infrastructure;
using System.Threading;

namespace Feex.Hangfire
{
    public class JobService
    {
        private readonly ILogger<JobService> _logger;
        private readonly IRecurringJobManager _recurringJobManager;
        private readonly AppKeys _settings;

        public JobService(ILogger<JobService> logger, IRecurringJobManager recurringJobManager, IOptions<AppKeys> option)
        {
            _logger = logger;
            _recurringJobManager = recurringJobManager;
            _settings = option.Value;
        }

        public void ScheduleRecurringJobs()
        {
            var recurringJobOptions = new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Local, // Set timezone
            };

            //every 5 minutes
            _recurringJobManager.AddOrUpdate("test_switch",() => TestSwitch(), "*/5 * * * *", recurringJobOptions);

        }

        
        [AutomaticRetry(Attempts = 0)]
        [DisableConcurrentExecution(0)]
        public async Task TestSwitch()
        {
            if (_settings.JobSwitch.TestSwitch?.ToLower() != "on")
            {
                return;
            }

            using var connection = JobStorage.Current.GetConnection();
            try
            {
                using var lockHandle = connection.AcquireDistributedLock("TestSwitchLock", TimeSpan.FromMinutes(20));

                //job logic can be called here

            }
            catch (DistributedLockTimeoutException)
            {
                _logger.LogInformation("ANOTHER INSTANCE IS ALREADY RUNNING. SKIPPING EXECUTION");
                //behaviour is EXPECTED and SAFE - we can't allow multiple instances running
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Critital error");
            }
        }
    }
}
