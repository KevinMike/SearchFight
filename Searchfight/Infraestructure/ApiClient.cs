using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Searchfight.Infraestructure
{
    public class ApiClient
    {
        public string EngineName { get; }
        private string _baseUrl { get; set; }
        private string _searchParam { get; set; }
        private Dictionary<string, string> _headers { get; set; }
        private Dictionary<string, string> _queryParams { get; set; }

        public ApiClient(string engineName, string baseUrl, string searchParam, Dictionary<string, string> headers, Dictionary<string, string> queryParams)
        {
            EngineName = engineName;
            _baseUrl = baseUrl;
            _searchParam = searchParam;
            _headers = headers;
            _queryParams = queryParams;
        }

        public async Task<string> SearchInEngine(string query)
        {
            using (HttpClient client = new HttpClient())
            {
                UriBuilder builder = new UriBuilder(_baseUrl);

                if (_headers != null)
                {
                    foreach (KeyValuePair<string, string> entry in _headers)
                    {
                        client.DefaultRequestHeaders.Add(entry.Key, entry.Value);
                    }
                }

                var queryParams = HttpUtility.ParseQueryString(builder.Query);
                queryParams[_searchParam] = query;

                if (_queryParams != null)
                {
                    foreach (KeyValuePair<string, string> entry in _queryParams)
                    {
                        queryParams[entry.Key] = entry.Value;
                    }
                }

                builder.Query = queryParams.ToString();
                string url = builder.ToString();

                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return null;
                }

            }
        }
    }
}
