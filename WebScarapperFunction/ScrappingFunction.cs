using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using WebScrapperFunction.OpenAIEmbeddingClient;
using WebScrapperFunction.Scrapper;

namespace WebScrapperFunction
{
    public class ScrappingFunction
    {
        private readonly ILogger _logger;
        private readonly IOpenAIClientService _openAIClientService;

        public ScrappingFunction(ILoggerFactory loggerFactory, IOpenAIClientService openAIClientService)
        {
            _logger = loggerFactory.CreateLogger<ScrappingFunction>();
            _openAIClientService = openAIClientService;
        }

        [Function("ScrappingFunction")]
        public async Task Run([TimerTrigger("*/1 * * * *")] TimerInfo timerTimer)
        {
            await _openAIClientService.ProcessTitles(new List<string>{"First title", "Second title"});
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
