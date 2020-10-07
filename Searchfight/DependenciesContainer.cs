using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Searchfight.Infraestructure;
using Searchfight.Services;

namespace Searchfight
{
    public static class DependenciesContainer
    {
        private static IConfiguration GetConfiguration()
        {
            IConfiguration Configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", false)
              .Build();
            return Configuration;
        }

        public static IServiceProvider ConfigureDependencies()
        {
            var services = new ServiceCollection()
                .AddTransient<ISearchService, SearchService>()
                .AddTransient<IApiClientFactory, ApiClientFactory>()
                .AddTransient((IServiceProvider arg) => GetConfiguration());

            services.AddLogging(configure => configure.AddConsole())
                .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information)
                .AddTransient<SearchService>();

            return services.BuildServiceProvider();
        }
    }
}
