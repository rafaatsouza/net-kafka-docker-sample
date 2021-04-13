using System;

namespace KafkaDockerSample.Core.Domain.Models
{
    public class Occurrence
    {
        public Guid Id { get; }
        
        public string Description { get; }

        public DateTime Date { get; }

        public Occurrence(Guid id, string description, DateTime date)
        {
            Id = id;
            Description = description;
            Date = date;
        }

        public Occurrence(string description, DateTime date)
        : this(Guid.NewGuid(), description, date)
        { }
    }
}