using Common.Configuration;
using NewsFetcherService.Contracts;
using NewsFetcherService.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        if (context.HostingEnvironment.IsDevelopment())
        {
            config.AddUserSecrets<Program>();
        }
    })
    .ConfigureServices((context, services) =>
    {
        services.Configure<RabbitMqOptions>(context.Configuration.GetSection(RabbitMqOptions.SectionName));
        services.AddHostedService<NewsFetcherBackgroundService>();
        services.AddSingleton<INewsFetcherBackgroundService, NewsFetcherBackgroundService>();
    })
    .Build();

await host.RunAsync();