using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace DemoService
{
    public class DemoMessageConsumer : IConsumer<IDemoMessage>
    {
        private readonly ILogger<DemoMessageConsumer> _logger;

        public DemoMessageConsumer(ILogger<DemoMessageConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<IDemoMessage> context)
        {
            _logger.LogInformation("Consuming message: {Text}", context.Message.Text);
            return Task.CompletedTask;
        }
    }
}