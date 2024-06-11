namespace BLL.Helpers
{
    public static class FallbackMessageHelper
    {
        public static readonly string FallbackMessage =
            "The requested information is not available in the retrieved data. Please try another query or topic.";
        public static bool DetectFallback(string modelResponse)
        {
            return modelResponse.Equals(FallbackMessage, StringComparison.InvariantCulture);
        }
    }
}
