namespace FrequentContentScrappingFunction.Models;

public class PageConfiguration
{
    public Guid Id { get; set; }
    public string ResourceLink { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string InitialPageNode { get; set; } = default!;
    public string ParsingPageNode { get; set; } = default!;
    public string InitialPageLink { get; set; } = default!;
    public int LastParsedItemIndex { get; set; }
}