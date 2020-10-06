using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Searchfight.Common
{
    public static class Utils
    {
        public static async Task<List<T>> ResolveTasksList<T>(List<Task<T>> tasks)
        {
            var retVal = await Task.WhenAll(tasks);
            return new List<T>(retVal);
        }

        public static string GetFieldFromJson(string json, string fieldPath)
        {
            JObject jObject = JObject.Parse(json);
            return (string)jObject.SelectToken(fieldPath);
        }
    }
}
