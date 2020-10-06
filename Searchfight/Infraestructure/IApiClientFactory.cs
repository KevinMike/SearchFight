using Searchfight.Models;

namespace Searchfight.Infraestructure
{
    public interface IApiClientFactory
    {
        ApiClient CreateApiClient(SearchEngineConfiguration searchEngineConfiguration);
    }
}
