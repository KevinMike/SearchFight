using System;
using System.Collections.Generic;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using Searchfight.Models;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Searchfight.Infraestructure;

namespace Searchfight.Test
{
    public class InfraestructureTests
    {
        private IServiceProvider _serviceProvider;
        private IConfiguration _configuration;
        private List<SearchEngineConfiguration> _searchEngines;

        [SetUp]
        public void Setup()
        {
            _serviceProvider = DependenciesContainer.ConfigureDependencies();
            _configuration = _serviceProvider.GetService<IConfiguration>();
            _searchEngines = new List<SearchEngineConfiguration>();
            var section = _configuration.GetSection("SearchEngines");
            section.Bind(_searchEngines);
        }

        [Test]
        public void ApiClient_Should_Return_Some_Value()
        {
            var googleEngine = _searchEngines.FirstOrDefault(p => p.Name == "Google");
            var apiClientFactory = _serviceProvider.GetService<IApiClientFactory>();
            var googleClient = apiClientFactory.CreateApiClient(googleEngine);
            var query = ".Net Core";

            var result = googleClient.SearchInEngine(query).Result;

            Assert.NotNull(result);
        }
    }
}