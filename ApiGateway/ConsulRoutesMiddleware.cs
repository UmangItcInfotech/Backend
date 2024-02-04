using Consul;
using Ocelot.Configuration.Builder;

namespace ApiGateway
{
    public class ConsulRoutesMiddleware
    {
        private readonly RequestDelegate _next;

        public ConsulRoutesMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IConsulClient consulClient)
        {
            var services = await consulClient.Catalog.Services();

            foreach (var serviceEntry in services.Response)
            {
                foreach (var service in serviceEntry.Value)
                {
                    var downstreamPath = $"/api/{service}";
                    var downstreamRoute = new DownstreamRouteBuilder()
                        .WithServiceName(service)
                        .WithDownstreamPathTemplate(downstreamPath)
                        .Build();

                    context.Items.Add($"ConsulRoute_{service}", downstreamRoute);
                }
            }

            await _next(context);
        }
    }

}
