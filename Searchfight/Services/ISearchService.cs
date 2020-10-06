using System.Collections.Generic;
using System.Threading.Tasks;
using Searchfight.Models;

namespace Searchfight.Services
{
    public interface ISearchService
    {
        Task<SearchServiceResponse> Search(List<string> queries);
    }
}
