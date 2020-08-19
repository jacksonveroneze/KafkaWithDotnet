using MediatR;

namespace KafkaWithDotnet.Producer.Api.Domain.Command
{
    public class ProducerCommand : IRequest<ProducerResult>
    {
        public string Data { get; set; }
    }
}
