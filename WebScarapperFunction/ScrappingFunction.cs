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
        public async Task Run([TimerTrigger("*/1 * * * *")] TimerInfo timerTimer)
        {
            var existingContent = new List<ResourcesModel>()
            {
                new()
                {
                    Title = "Test1",
                    UrlPath = "google.com"
                },
                new()
                {
                    Title = "Test2",
                    UrlPath = "google.com"
                }
            };
            //await _openAIClientService.EmbedResCollectionAsync(listEmbeddings);
            //var existingContent = await WebScrapper.ParseReferences();

            await _resourcesModelAccessor.UpdateResources(existingContent);
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
