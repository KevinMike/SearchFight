using System.Collections.Generic;

namespace Searchfight.Models
{
    public class SearchEngineConfiguration
    {
        public string Name { get; set; }
        public string ApiUrl { get; set; }
        public string SearchParam { get; set; }
        public string ResultPath { get; set; }
        public Dictionary<string,string> Params { get; set; }
        public Dictionary<string, string> Headers { get; set; }

        public SearchEngineConfiguration()
        {
            Params = new Dictionary<string, string>();
            Headers = new Dictionary<string, string>();
        }
    }
}
