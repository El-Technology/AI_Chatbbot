namespace FrequentContentScrappingFunction.Models;

public class AppendModel
{
    public string FilepathToAppend { get; set; } = string.Empty;
    public MemoryStream MemoryStream { get; set; } = null!;
}