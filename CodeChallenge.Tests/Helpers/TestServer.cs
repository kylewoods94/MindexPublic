using System;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Testing;

namespace CodeCodeChallenge.Tests.Integration.Helpers
{
    public class TestServer : IDisposable, IAsyncDisposable
    {
        private WebApplicationFactory<Program> applicationFactory;

        public TestServer()
        {
            applicationFactory = new WebApplicationFactory<Program>();
        }

        public HttpClient NewClient()
        {
            var client = applicationFactory.CreateClient();
            client.DefaultRequestHeaders.Add("x-api-key", "3F761822-7EC2-49E4-B5A8-A9F49B88FD93");
            return client;
        }

        public ValueTask DisposeAsync()
        {
            return ((IAsyncDisposable)applicationFactory).DisposeAsync();
        }

        public void Dispose()
        {
            ((IDisposable)applicationFactory).Dispose();
        }
    }
}
