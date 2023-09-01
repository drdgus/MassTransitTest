using MassTransit;
using MassTransitTest;
using Microsoft.AspNetCore.Mvc;

namespace SimplePublisher.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageProcessedProducer : ControllerBase
    {
        private readonly ILogger<MessageProcessedProducer> _logger;
        private readonly ITopicProducer<MessageProcessed> _publishEndpoint;

        public MessageProcessedProducer(ILogger<MessageProcessedProducer> logger, ITopicProducer<MessageProcessed> publishEndpoint)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet(Name = "Produce")]
        public async ValueTask<IActionResult> Get()
        {
            await _publishEndpoint.Produce(new MessageProcessed("Hello!"));
            return Ok();
        }
    }
}