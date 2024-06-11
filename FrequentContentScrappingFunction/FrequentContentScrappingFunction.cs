using FrequentContentScrappingFunction.Services.AdditionalContentService;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FrequentContentScrappingFunction;

public class FrequentContentScrappingFunction
{
    private readonly IAdditionalContentService _additionalContentService;
    private readonly ILogger _logger;

    public FrequentContentScrappingFunction(ILoggerFactory loggerFactory,
        IAdditionalContentService additionalContentService)
    {
        _logger = loggerFactory.CreateLogger<FrequentContentScrappingFunction>();
        _additionalContentService = additionalContentService;
    }

    [Function("FrequentContentScrappingFunction")]
    public async Task Run([TimerTrigger("*/3 * * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

        await _additionalContentService.ParseFrequentContent();

        if (myTimer.ScheduleStatus is not null)
            _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
    }
}