namespace Searchfight.Models
{
    public class SearchEngineResponse
    {
        public string SearchEngineName { get; }
        public string Query { get; }
        public long NumberOfResults { get; }
        
        public SearchEngineResponse(string searchEngineName, string query, long numberOfResults)
        {
            SearchEngineName = searchEngineName;
            Query = query;
            NumberOfResults = numberOfResults;
        }
    }
}
