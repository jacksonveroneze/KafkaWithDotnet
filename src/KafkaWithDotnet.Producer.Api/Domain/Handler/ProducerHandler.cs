using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using KafkaWithDotnet.Producer.Api.Domain.Command;
using MediatR;
using Microsoft.Extensions.Logging;

namespace KafkaWithDotnet.Producer.Api.Domain.Handler
{
    public class ProducerHandler : IRequestHandler<ProducerCommand, ProducerResult>
    {
        private readonly ILogger<ProducerHandler> _logger;
        private readonly ProducerConfig _config;

        //
        // Summary:
        //     /// Method responsible for initializing the handler. ///
        //
        // Parameters:
        //   logger:
        //     The logger param.
        //
        //   config:
        //     The config param.
        //
        public ProducerHandler(ILogger<ProducerHandler> logger, ProducerConfig config)
        {
            _logger = logger;
            _config = config;
        }

        //
        // Summary:
        //     /// Method responsible for handle. ///
        //
        // Parameters:
        //   request:
        //     The request param.
        //
        //   cancellationToken:
        //     The cancellationToken param.
        //
        public async Task<ProducerResult> Handle(ProducerCommand request, CancellationToken cancellationToken)
        {
            string data = JsonSerializer.Serialize(request);

            _logger.LogInformation("Producer: {0}", data);

            using IProducer<Null, string> producer = new ProducerBuilder<Null, string>(_config).Build();

            DeliveryResult<Null, string> result = await producer.ProduceAsync("new-producer",
                new Message<Null, string> {Value = data});

            return new ProducerResult() {Result = result.Topic};
        }
    }
}