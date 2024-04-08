using System.Collections.Generic;

namespace AIAzureChatbot.Models;

public class RootObject
{
    public List<Citation> Citations { get; set; }
    public string Intent { get; set; }
}