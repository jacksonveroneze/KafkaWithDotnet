using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;
using KafkaWithDotnet.Producer.Api.Domain.Command;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KafkaWithDotnet.Producer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProducerController : ControllerBase
    {
        private readonly ILogger<ProducerController> _logger;
        private readonly ProducerConfig _config;

        //
        // Summary:
        //     /// Method responsible for initializing the controller. ///
        //
        // Parameters:
        //   logger:
        //     The logger param.
        //
        public ProducerController(ILogger<ProducerController> logger)
        {
            _logger = logger;

            _config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };
        }

        //
        // Summary:
        //     /// Method responsible for action: New (POST). ///
        //
        // Parameters:
        //   command:
        //     The command param.
        //
        [HttpPost("new")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ProducerResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBodyAttribute] ProducerCommand command)
        {
            using IProducer<Null, string> producer = new ProducerBuilder<Null, string>(_config).Build();

            DeliveryResult<Null, string> result = await producer.ProduceAsync("new-producer",
                new Message<Null, string> {Value = JsonSerializer.Serialize(command)});

            return Ok(new ProducerResult());
        }
    }
}