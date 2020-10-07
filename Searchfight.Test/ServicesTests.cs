using System;
using System.Collections.Generic;
using NUnit.Framework;
using Searchfight.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Searchfight.Test
{
    public class ServicesTests
    {
        private IServiceProvider _serviceProvider;
        private ISearchService _searchService;

        [SetUp]
        public void Setup()
        {
            _serviceProvider = DependenciesContainer.ConfigureDependencies();
            _searchService = _serviceProvider.GetService<ISearchService>();
        }

        [Test]
        public void Should_Have_A_Winner()
        {
            var queries = new List<string>() {
                { ".net" },
                { "java" }
            };

            var result = _searchService.Search(queries).Result;

            Assert.NotNull(result.Winner);
        }

        [Test]
        public void Winner_Should_Be_Same_Regardless_Queries_Order()
        {
            var queries = new List<string>() {
                { ".net" },
                { "java" }
            };

            var result1 = _searchService.Search(queries).Result;
            queries.Reverse();
            var result2 = _searchService.Search(queries).Result;

            Assert.IsTrue(result1.Winner == result2.Winner);
        }
    }
}
