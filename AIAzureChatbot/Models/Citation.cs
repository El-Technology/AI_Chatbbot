using System.Collections.Generic;

namespace AIAzureChatbot.Models;

public class Citation
{
    public string Content { get; set; }
    public string Id { get; set; }
    public string Title { get; set; }
    public string FilePath { get; set; }
    public string Url { get; set; }
    public Dictionary<string, string> Metadata { get; set; }
    public string ChunkId { get; set; }
}