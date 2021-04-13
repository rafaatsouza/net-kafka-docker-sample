using KafkaDockerSample.Core.Domain.Services;
using KafkaDockerSample.Ui.Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace KafkaDockerSample.Ui.Api.Controllers
{
    /// <summary>
    /// Controller responsible for register a new occurrence
    /// </summary>
    [Route("api/occurrences")]
    [ApiController]
    public class OccurrencesController : ControllerBase
    {
        private readonly IOccurrenceService occurrenceService;

        public OccurrencesController(IOccurrenceService occurrenceService)
        {
            this.occurrenceService = occurrenceService
                ?? throw new ArgumentNullException(nameof(occurrenceService));
        }

        /// <summary>
        /// Registers a new occurrence.
        /// </summary>
        /// <param name="request">Object containing the occurrence content</param>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpPost(Name = "RegisterOccurrence")]
        public async Task<IActionResult> RegisterOccurenceAsync(
            [FromBody] RegisterOccurrence request, [FromQuery] int maxRetry = 3)
        {
            await occurrenceService.RegisterOccurrenceAsync(
                request.Description, request.Date, maxRetry);

            return Ok();
        }
    }
}
