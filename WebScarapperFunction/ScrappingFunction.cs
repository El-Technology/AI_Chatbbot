using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace WebScarapperFunction
{
    public class ScrappingFunction
    {
        private readonly ILogger _logger;

        public ScrappingFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ScrappingFunction>();
        }

        [Function("ScrappingFunction")]
        public void Run([TimerTrigger("0 0 * * 6")] TimerInfo timerTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            _logger.LogInformation($"Next timer schedule at: {timerTimer.ScheduleStatus.Next}");
        }
    }

    public class TimerInfo
    {
        public ScheduleStatus ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }

    public class ScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
