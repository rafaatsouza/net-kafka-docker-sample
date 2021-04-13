using System;

namespace KafkaDockerSample.Infrastructure.DistributedStreamer
{
    public class DistributedStreamerConfiguration
    {
        public string Server { get; }

        public DistributedStreamerConfiguration(string server)
        {
            Server = !string.IsNullOrEmpty(server) ? server
                : throw new ArgumentException("Null or empty", nameof(Server));
        }
    }
}