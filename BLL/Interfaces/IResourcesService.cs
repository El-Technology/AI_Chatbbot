using BLL.Dtos;

namespace BLL.Interfaces;

public interface IResourcesService
{
    Task<List<ResourcesModelDto>> GetRelatedResourcesAsync(string textUserInput);
}