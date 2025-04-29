using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using NewsFetcherService.Contracts;
using Common.Configuration;
using Microsoft.Extensions.Options;

namespace NewsFetcherService.Services
{
    public class NewsFetcherBackgroundService : BackgroundService, INewsFetcherBackgroundService
    {
        private readonly ILogger<NewsFetcherBackgroundService> _logger;
        private IConnection? _connection;
        private IChannel? _channel;
        private readonly RabbitMqOptions _rabbitMqOptions;
        private string _exchangeName = string.Empty;
        private string _routingKey = string.Empty;
        private string _queueName = string.Empty;
        private string _clientName = string.Empty;

        public NewsFetcherBackgroundService(ILogger<NewsFetcherBackgroundService> logger, IOptions<RabbitMqOptions> options)
        {
            _logger = logger;
            _rabbitMqOptions = options.Value;
        }

        public async Task InitializeRabbitMqAsync()
        {
            var uri = _rabbitMqOptions.Uri;
            _exchangeName = _rabbitMqOptions.ExchangeName ?? "news.exchange";
            _routingKey = _rabbitMqOptions.RoutingKey ?? "news.routing.key";
            _queueName = _rabbitMqOptions.QueueName ?? "news.queue";
            _clientName = _rabbitMqOptions.ClientProvidedName ?? "NewsFetcherServiceClient";

            var factory = new ConnectionFactory
            {
                Uri = new Uri(uri),
                ClientProvidedName = _clientName,
                ConsumerDispatchConcurrency = 1,
            };

            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            await _channel.ExchangeDeclareAsync(_exchangeName, ExchangeType.Direct, durable: true);
            await _channel.QueueDeclareAsync(_queueName, durable: true, exclusive: false, autoDelete: false);
            await _channel.QueueBindAsync(_queueName, _exchangeName, _routingKey);

            _logger.LogInformation("RabbitMQ initialized successfully. Exchange: {Exchange}, Queue: {Queue}, RoutingKey: {RoutingKey}",
                _exchangeName, _queueName, _routingKey);
        }

        public async Task PublishFakeNewsAsync(CancellationToken stoppingToken)
        {
            var article = GenerateFakeArticle();
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(article));

            await _channel!.BasicPublishAsync(
                exchange: _exchangeName,
                routingKey: _routingKey,
                mandatory: true,
                body: body);

            _logger.LogInformation("Published article: {Title} at {TimeUtc}", article.Title, DateTime.UtcNow);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting NewsFetcherBackgroundService...");

            await InitializeRabbitMqAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                await PublishFakeNewsAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        private NewsArticle GenerateFakeArticle()
        {
            return new NewsArticle
            {
                ArticleId = Guid.NewGuid().ToString(),
                Title = $"Breaking News at {DateTime.UtcNow:HH:mm:ss}",
                Content = "Simulated article content for testing purposes."
            };
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping NewsFetcherBackgroundService...");

            if (_channel is not null)
                await _channel.CloseAsync();

            if (_connection is not null)
                await _connection.CloseAsync();

            await base.StopAsync(cancellationToken);
        }
    }

    public class NewsArticle
    {
        public string ArticleId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
