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
        //   config:
        //     The config param.
        //
        public ProducerController(ILogger<ProducerController> logger, ProducerConfig config)
        {
            _logger = logger;
            _config = config;
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
            string data = JsonSerializer.Serialize(command);

            _logger.LogInformation("Producer: {0}", data);

            using IProducer<Null, string> producer = new ProducerBuilder<Null, string>(_config).Build();

            DeliveryResult<Null, string> result = await producer.ProduceAsync("new-producer",
                new Message<Null, string> {Value = data});

            return Ok(new ProducerResult() { Result = result.Topic});
        }
    }
}