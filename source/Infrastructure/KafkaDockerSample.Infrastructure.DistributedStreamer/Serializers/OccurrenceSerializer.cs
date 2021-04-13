using Confluent.Kafka;
using KafkaDockerSample.Core.Domain.Models;
using Newtonsoft.Json;
using System.Text;

namespace KafkaDockerSample.Infrastructure.DistributedStreamer.Serializers
{
    public class OccurrenceSerializer : ISerializer<Occurrence>
    {
        public byte[] Serialize(Occurrence data, SerializationContext context)
        {
            string jsonContent = JsonConvert.SerializeObject(data);

            return Encoding.ASCII.GetBytes(jsonContent);
        }
    }
}