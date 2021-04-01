using KafkaDockerSample.Core.Domain.Services;
using KafkaDockerSample.Ui.Api.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KafkaDockerSample.Ui.Api.Controllers
{
    /// <summary>
    /// Controller responsible for create a new message
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageSenderService senderService;

        public MessageController(IMessageSenderService senderService)
        {
            this.senderService = senderService
                ?? throw new ArgumentNullException(nameof(senderService));
        }

        /// <summary>
        /// Inserts new object.
        /// </summary>
        /// <param name="request">Object containing the message content</param>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpPost(Name = "SendMessage")]
        public async Task<IActionResult> SendMessageAsync(
            [FromBody] SendMessageRequest request)
        {
            if (string.IsNullOrEmpty(request?.Message))
                return StatusCode(400);

            await senderService.SendMessageAsync(request.Message);

            return Ok();
        }
    }
}
