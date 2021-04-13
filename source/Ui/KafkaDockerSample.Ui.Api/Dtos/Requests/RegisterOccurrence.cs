using System;

namespace KafkaDockerSample.Ui.Api.Dtos
{
    public class RegisterOccurrence
    {
        /// <summary>
        /// Occurrence's description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Occurrence's date time
        /// </summary>
        public DateTime Date { get; set; }
    }
}
