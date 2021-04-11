using System;

namespace KafkaDockerSample.Infrastructure.MessageStreamer
{
    public class MessageStreamerConfiguration
    {
        public string Server { get; }

        public MessageStreamerConfiguration(string server)
        {
            Server = !string.IsNullOrEmpty(server) ? server
                : throw new ArgumentException("Null or empty", nameof(Server));
        }
    }
}