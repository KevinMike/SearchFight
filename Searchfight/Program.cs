using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Searchfight.Infraestructure;
using Searchfight.Services;

namespace Searchfight
{
    class Program
    {
        private static IConfiguration GetConfiguration()
        {

            IConfiguration Configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", false)
              .Build();
            return Configuration;
        }

        private static IServiceProvider ConfigureDependencies()
        {
            var services = new ServiceCollection()
                .AddTransient<ISearchService, SearchService>()
                .AddTransient<IApiClientFactory, ApiClientFactory>()
                .AddTransient((IServiceProvider arg) => GetConfiguration());

            return services.BuildServiceProvider();
        }

        static void Main(string[] args)
        {
            var queries = new List<string>(args);

            if(queries.Count == 0)
            {
                Console.WriteLine($"You must provide at least 2 search words as anguments");
                return;
            }

            var serviceProvider = ConfigureDependencies();
            var appService = serviceProvider.GetService<ISearchService>();
            var result = appService.Search(queries).Result;

            foreach(var failed in result.RequestFailed)
            {
                Console.WriteLine($"* {failed}");
            }

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
