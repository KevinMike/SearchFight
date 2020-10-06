using Searchfight.Models;

namespace Searchfight.Infraestructure
{
    public class ApiClientFactory : IApiClientFactory
    {
        public ApiClient CreateApiClient(SearchEngineConfiguration searchEngineConfiguration)
        {
            return new ApiClient(searchEngineConfiguration.Name, searchEngineConfiguration.ApiUrl, searchEngineConfiguration.SearchParam, searchEngineConfiguration.Headers, searchEngineConfiguration.Params);
        }
    }
}
