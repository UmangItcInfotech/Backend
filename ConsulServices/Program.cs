using Consul;
using ConsulServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
/*builder.Services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
{
    consulConfig.Address = new Uri(builder.Configuration.GetSection("ConsulConf:ConsulUri").Value);
}));
builder.Services.AddSingleton<IHostedService, ConsulHostedService>();
builder.Services.Configure<ConsulConfig>(builder.Configuration.GetSection("Consul"));*/


var app = builder.Build();

app.Run();
