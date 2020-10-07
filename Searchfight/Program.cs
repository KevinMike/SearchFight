using System;
using System.Collections.Generic;
using Searchfight.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Searchfight
{
    class Program
    {
        static void Main(string[] args)
        {
            var queries = new List<string>(args);

            if(queries.Count < 2)
            {
                Console.WriteLine($"You must provide at least 2 search words as anguments");
                return;
            }

            Console.WriteLine("Searching...");

            IServiceProvider serviceProvider = DependenciesContainer.ConfigureDependencies();
            var appService = serviceProvider.GetService<ISearchService>();
            var result = appService.Search(queries).Result;

            foreach (KeyValuePair<string, Dictionary<string,long>> entry in result.ResultsPerQuery)
            {
                Console.WriteLine($"Results for {entry.Key}");
                foreach(KeyValuePair<string,long> engineDetail in entry.Value)
                {
                    Console.WriteLine($"     => {engineDetail.Key} found {engineDetail.Value} results");
                }
            }

            foreach (KeyValuePair<string, string> entry in result.WinnerPerSearchEngine)
            {
                Console.WriteLine($"{entry.Key} winner is {entry.Value}");
            }

            Console.WriteLine($"The final winner is {result.Winner}");
        }
    }
}
