using KafkaDockerSample.Core.Domain.Services;
using KafkaDockerSample.Ui.Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace KafkaDockerSample.Ui.Api.Controllers
{
    /// <summary>
    /// Controller responsible for create a new message
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService messageService;

        public MessageController(IMessageService messageService)
        {
            this.messageService = messageService
                ?? throw new ArgumentNullException(nameof(messageService));
        }

        /// <summary>
        /// Inserts new object.
        /// </summary>
        /// <param name="request">Object containing the message content</param>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpPost(Name = "SendMessage")]
        public async Task<IActionResult> SendMessageAsync(
            [FromBody] SendMessage request)
        {
            await messageService.SendMessageAsync(request.Message);

            return Ok();
        }
    }
}
