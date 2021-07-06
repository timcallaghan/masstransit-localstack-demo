using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DemoService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IBus _bus;

        public Worker(ILogger<Worker> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var message = new DemoMessage(DateTimeOffset.Now.ToString());
                _logger.LogInformation("Publishing message: {Text}", message.Text);
                
                await _bus.Publish(message, stoppingToken);
                
                await Task.Delay(3000, stoppingToken);
            }
        }
    }
}
