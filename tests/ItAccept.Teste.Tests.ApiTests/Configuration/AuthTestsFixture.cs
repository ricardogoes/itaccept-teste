using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using Xunit;

namespace ItAccept.Teste.Tests.ApiTests.Configuration
{
    [CollectionDefinition(nameof(AuthTestsCollection))]
    public class AuthTestsCollection : ICollectionFixture<AuthTestsFixture>
    { }

    public class AuthTestsFixture
    {
        public readonly HttpClient HttpClient;

        public AuthTestsFixture()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");

            var webAppFactory = new WebApplicationFactory<Program>();
            HttpClient = webAppFactory.CreateDefaultClient();
        }
    }
}
