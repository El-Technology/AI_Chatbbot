using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using WebScrapperFunction.Scrapper;

namespace WebScrapperFunction
{
    public class ScrappingFunction
    {
        private readonly ILogger _logger;

        public ScrappingFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ScrappingFunction>();
        }

        [Function("ScrappingFunction")]
        public async Task Run([TimerTrigger("*/5 * * * *")] TimerInfo timerTimer)
        {
            await WebScrapper.ParseReferences();
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            _logger.LogInformation($"Next timer schedule at: {timerTimer.ScheduleStatus.Next}");
        }
    }

    public class TimerInfo
    {
        public ScheduleStatus ScheduleStatus { get; set; } = null!;

        public bool IsPastDue { get; set; }
    }

    public class ScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
