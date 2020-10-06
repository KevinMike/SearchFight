using System.Collections.Generic;

namespace Searchfight.Models
{
    public class SearchServiceResponse
    {
        public string Winner { get; }
        public Dictionary<string,string> WinnerPerSearchEngine { get; set; }
        public Dictionary<string,Dictionary<string,long>> ResultsPerQuery { get; set; }

        public SearchServiceResponse(string winner, Dictionary<string, string> winnerPerSearchEngine, Dictionary<string, Dictionary<string, long>> resultsPerQuery)
        {
            Winner = winner;
            WinnerPerSearchEngine = winnerPerSearchEngine;
            ResultsPerQuery = resultsPerQuery;
        }
    }
}
