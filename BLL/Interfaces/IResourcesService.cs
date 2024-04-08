using DLL.Models;

namespace BLL.Interfaces;

public interface IResourcesService
{
    Task<List<ResourcesModel>> GetResources(string textUserInput);
}