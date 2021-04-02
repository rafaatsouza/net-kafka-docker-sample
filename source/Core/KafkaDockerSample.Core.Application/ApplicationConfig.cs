namespace KafkaDockerSample.Core.Application
{
    public class ApplicationConfig
    {
        public string KafkaTopic { get; set; }

        public string KafkaServer { get; set; }

        public int MaxSendRetry { get; set; }
    }
}
