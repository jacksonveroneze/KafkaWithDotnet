using System.Net.Mime;
using System.Threading.Tasks;
using KafkaWithDotnet.Producer.Api.Domain.Command;
using MediatR;
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
        private readonly IMediator _mediator;

        //
        // Summary:
        //     /// Method responsible for initializing the controller. ///
        //
        // Parameters:
        //   logger:
        //     The logger param.
        //
        //   mediator:
        //     The mediator param.
        //
        public ProducerController(ILogger<ProducerController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        //
        // Summary:
        //     /// Method responsible for action: New (POST). ///
        //
        // Parameters:
        //   request:
        //     The request param.
        //
        [HttpPost("new")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ProducerResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBodyAttribute] ProducerCommand request)
            => Ok(await _mediator.Send(request));
    }
}