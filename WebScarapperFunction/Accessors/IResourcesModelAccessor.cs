using WebScrapperFunction.Models;

namespace WebScrapperFunction.Accessors
{
    public interface IResourcesModelAccessor
    {
        Task UpdateResources(List<ResourcesModel> parsedResources);
    }
}
