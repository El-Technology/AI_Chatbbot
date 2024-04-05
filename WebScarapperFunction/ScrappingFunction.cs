using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using WebScrapperFunction.Accessors;
using WebScrapperFunction.Accessors.Models;
using WebScrapperFunction.OpenAIEmbeddingClient;
using WebScrapperFunction.Scrapper;

namespace WebScrapperFunction
{
    public class ScrappingFunction
    {
        private readonly ILogger _logger;
        private readonly IResourcesModelAccessor _resourcesModelAccessor;

        public ScrappingFunction(ILoggerFactory loggerFactory, IResourcesModelAccessor resourcesModelAccessor)
        {
            _logger = loggerFactory.CreateLogger<ScrappingFunction>();
            _resourcesModelAccessor = resourcesModelAccessor;
        }

        [Function("ScrappingFunction")]
        public async Task Run([TimerTrigger("*/5 * * * *")] TimerInfo timerTimer)
        {
            var existingContent = await WebScrapper.ParseReferences();
            await _resourcesModelAccessor.UpdateResources(existingContent);
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.UtcNow}");
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
