using MassTransit;

namespace MassTransitTest
{
    public class MessageProcessedConsumer : IConsumer<MessageProcessed>
    {
        private readonly ILogger<MessageProcessedConsumer> _logger;

        public MessageProcessedConsumer(ILogger<MessageProcessedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<MessageProcessed> context)
        {
            var btn = context.Message.Button;
            _logger.LogCritical("Button pressed {Button}", btn);

            return Task.CompletedTask;
        }
    }
}
