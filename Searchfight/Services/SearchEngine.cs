using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Searchfight.Common;
using Searchfight.Infraestructure;
using Searchfight.Models;

namespace Searchfight.Services
{
    public class SearchService : ISearchService
    {
        private readonly IApiClientFactory _apiClientFactory;
        private readonly ILogger _logger;
        private List<SearchEngineConfiguration> _searchEngines { get; set; }

        public SearchService(IApiClientFactory apiClientFactory, IConfiguration configuration, ILogger<SearchService> logger)
        {
            _searchEngines = new List<SearchEngineConfiguration>();
            _apiClientFactory = apiClientFactory;
            _logger = logger;

            var section = configuration.GetSection("SearchEngines");
            section.Bind(_searchEngines);
        }

        public async Task<SearchServiceResponse> Search(List<string> queries)
        {
            var totalNumberOfResulsPerQuery = new Dictionary<string, long>();
            var detailNumberOfResultsPerQuery = new Dictionary<string, Dictionary<string, long>>();
            var winnerPerSearchEngine = new Dictionary<string, string>();
            var winner = string.Empty;
            var requests = new List<Task<SearchEngineResponse>>();

            foreach (var query in queries)
            {
                foreach (var searchEngine in _searchEngines)
                {
                    ApiClient searchEngineClient = _apiClientFactory.CreateApiClient(searchEngine);
                    requests.Add(GetNumberOfResults(query, searchEngineClient, searchEngine.ResultPath));
                }
            }

            var responses = await Utils.ResolveTasksList(requests);

            foreach (var response in responses)
            {
                if (winner == string.Empty)
                {
                    winner = response.Query;
                }

                if (!winnerPerSearchEngine.ContainsKey(response.SearchEngineName))
                {
                    winnerPerSearchEngine[response.SearchEngineName] = response.Query;
                }

                if (detailNumberOfResultsPerQuery.ContainsKey(response.Query))
                {
                    detailNumberOfResultsPerQuery[response.Query][response.SearchEngineName] = response.NumberOfResults;
                }
                else
                {
                    detailNumberOfResultsPerQuery[response.Query] = new Dictionary<string, long>() { { response.SearchEngineName, response.NumberOfResults } };
                }


                if (totalNumberOfResulsPerQuery.ContainsKey(response.Query))
                {
                    totalNumberOfResulsPerQuery[response.Query] += response.NumberOfResults;
                }
                else
                {
                    totalNumberOfResulsPerQuery[response.Query] = response.NumberOfResults;
                }

                if (totalNumberOfResulsPerQuery[winner] < totalNumberOfResulsPerQuery[response.Query]) {
                    winner = response.Query;
                }

                if(winnerPerSearchEngine[response.SearchEngineName] != response.Query)
                {
                    if(detailNumberOfResultsPerQuery[winnerPerSearchEngine[response.SearchEngineName]][response.SearchEngineName]
                        < detailNumberOfResultsPerQuery[response.Query][response.SearchEngineName])
                    {
                        winnerPerSearchEngine[response.SearchEngineName] = response.Query;
                    }
                }
            }

            return new SearchServiceResponse(winner, winnerPerSearchEngine, detailNumberOfResultsPerQuery) ;
        }

        private async Task<SearchEngineResponse> GetNumberOfResults(string query, ApiClient searchEngineClient, string resultPath)
        {
            try
            {
                long results;
                string jsonResult = await searchEngineClient.SearchInEngine(query);
                string numberOfResults = Utils.GetFieldFromJson(jsonResult, resultPath);

                if (Int64.TryParse(numberOfResults, out results))
                {
                    return new SearchEngineResponse(searchEngineClient.EngineName, query, results);
                }
                else
                {
                    _logger.Log(LogLevel.Warning, $"Could not get results for {query} in {searchEngineClient.EngineName}");
                    return new SearchEngineResponse(searchEngineClient.EngineName, query, 0);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, $"The {searchEngineClient.EngineName} request of {query} faild due to: {ex.Message}");
                return new SearchEngineResponse(searchEngineClient.EngineName, query, 0);
            }
            
        }
    }
}
