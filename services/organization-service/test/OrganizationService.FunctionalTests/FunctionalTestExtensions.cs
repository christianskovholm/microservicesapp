using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OrganizationService.Application;
using OrganizationService.Domain.Aggregates.Organization;
using OrganizationService.Infrastructure;

namespace OrganizationService.FunctionalTests
{
    public static class FunctionalTestExtensions
    {
        public static TestServer CreateTestServer()
        {
            var hostBuilder = new WebHostBuilder().UseStartup<Startup>();
            return new TestServer(hostBuilder);
        }

        public static Organization CreateOrganization(this TestServer testServer, Organization organization)
        {
            using (var scope = testServer.Host.Services.CreateScope())
            {
                using(var context = scope.ServiceProvider.GetService<OrganizationDbContext>())
                {
                    context.Organizations.Add(organization);
                    context.SaveChangesAsync().Wait();
                }
            }

            return organization;
        }

        public static StringContent ToStringContent(this object command) => new StringContent(JsonConvert.SerializeObject(command), UTF8Encoding.UTF8, "application/json");
    }
}