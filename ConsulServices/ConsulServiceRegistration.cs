using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsulServices
{
    public class ConsulServiceRegistration
    {
        private readonly IConfiguration _configuration;
        private readonly IConsulClient _consulClient;
        private readonly ILogger<ConsulServiceRegistration> _logger;

        public ConsulServiceRegistration(IConfiguration configuration, IConsulClient consulClient, ILogger<ConsulServiceRegistration> logger)
        {
            _configuration = configuration;
            _consulClient = consulClient;
            _logger = logger;
        }

        public async Task RegisterServiceAsync(CancellationToken cancellationToken)
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
    }
}
