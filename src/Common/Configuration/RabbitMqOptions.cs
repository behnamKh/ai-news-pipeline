namespace Common.Configuration
{
    public class RabbitMqOptions
    {
        public const string SectionName = "RabbitMQ";

        public string Uri { get; set; } = string.Empty;
        public string ExchangeName { get; set; } = string.Empty;
        public string RoutingKey { get; set; } = string.Empty;
        public string QueueName { get; set; } = string.Empty;
        public string ClientProvidedName { get; set; } = string.Empty;
    }
}
