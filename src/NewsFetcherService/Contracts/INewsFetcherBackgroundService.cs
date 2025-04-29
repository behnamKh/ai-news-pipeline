namespace NewsFetcherService.Contracts
{
    public interface INewsFetcherBackgroundService
    {
        Task InitializeRabbitMqAsync();
        Task PublishFakeNewsAsync(CancellationToken stoppingToken);
    }
}
