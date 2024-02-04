using ApiGateway;
using Consul;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConsulClient, ConsulClient>(); // Add ConsulClient as a singleton

builder.Services.AddOcelot()
    .AddConsul()
    .AddConfigStoredInConsul();

var app = builder.Build();

app.UseMiddleware<ConsulRoutesMiddleware>();

app.UseOcelot().Wait();

app.Run();
