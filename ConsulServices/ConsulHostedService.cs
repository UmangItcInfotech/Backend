using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsulServices
{
    public class ConsulHostedService : IHostedService
    {
        private readonly IConfiguration _configuration;
        private readonly IConsulClient _consulClient;
        private readonly ILogger<ConsulHostedService> _logger;

        public ConsulHostedService(IConfiguration configuration, IConsulClient consulClient, ILogger<ConsulHostedService> logger)
        {
            _configuration = configuration;
            _consulClient = consulClient;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var serviceConfig = _configuration.GetSection("Consul").Get<ConsulConfig>();
            var registration = new AgentServiceRegistration
            {
                ID = serviceConfig.ServiceId,
                Name = serviceConfig.ServiceName,
                Address = serviceConfig.ServiceHost,
                Port = serviceConfig.ServicePort
            };

            _logger.LogInformation($"Registering service with Consul: {registration.Name}");

            await _consulClient.Agent.ServiceDeregister(registration.ID, cancellationToken);
            await _consulClient.Agent.ServiceRegister(registration, cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            var serviceConfig = _configuration.GetSection("Consul").Get<ConsulConfig>();
            var registration = new AgentServiceRegistration { ID = serviceConfig.ServiceId };

            _logger.LogInformation($"Deregistering service from Consul: {registration.ID}");

            await _consulClient.Agent.ServiceDeregister(registration.ID, cancellationToken);
        }
    }
}
