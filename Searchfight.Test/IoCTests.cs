using System;
using System.Collections.Generic;
using NUnit.Framework;
using Searchfight.Services;
using Microsoft.Extensions.DependencyInjection;
using Searchfight.Infraestructure;
using System.Reflection;

namespace Searchfight.Test
{
    public class IoCTests
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
        public void Get_ISearchService_Instance()
        {
            var instance = _serviceProvider.GetService<ISearchService>();

            Type type = instance.GetType();
            var comparison = type.Equals(typeof(SearchService));

            Assert.IsTrue(comparison);
        }

        [Test]
        public void Get_IApiClientFactory_Instance()
        {
            var instance = _serviceProvider.GetService<IApiClientFactory>();

            Type type = instance.GetType();
            var comparison = type.Equals(typeof(ApiClientFactory));

            Assert.IsTrue(comparison);
        }

    }
}
