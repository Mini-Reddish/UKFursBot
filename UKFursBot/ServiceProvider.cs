using System.Reflection;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UKFursBot.Commands;
using UKFursBot.Context;
using UKFursBot.UserJoined;

namespace UKFursBot;

public static class ServiceProvider
{
    private static IServiceProvider? _instance;
    public static IServiceProvider Instance => _instance ?? CreateServiceProvider();

    private static IServiceProvider CreateServiceProvider()
    {
        if (_instance == null)
        {
           
            var service = new ServiceCollection();
            
            service.AddConfiguration();
            service.AddDbContext<UKFursBotDbContext>();
            service.AddDiscordClient();
            service.AddSingletonOfType<ISlashCommand>();
            service.AddSingletonOfType<IUserJoinedHandler>();
            service.AddSingleton<SocketMessageChannelManager>();
            
            _instance = service.BuildServiceProvider();
        }

        return _instance;
    }
    
   
}

static class ServiceCollectionExtensions
{
    public static void AddConfiguration(this ServiceCollection service)
    {
        var environmentName = Environment.GetEnvironmentVariable("ENVIRONMENT");
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appSettings.json", optional: false)
            .AddJsonFile($"appSettings.{environmentName}.json", optional: true)
            .AddEnvironmentVariables();
            
        IConfiguration configuration = builder.Build();
        service.AddSingleton(configuration);
    }

    public static void AddDiscordClient(this ServiceCollection service)
    {
        var client = new DiscordSocketClient(new DiscordSocketConfig()
        {
            LogLevel = LogSeverity.Debug,
            LogGatewayIntentWarnings = true,
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers | GatewayIntents.GuildBans,
            MessageCacheSize = 100
        });

        service.AddSingleton(client);
    }
    public static void AddSingletonOfType<T>(this ServiceCollection service)
    {
        var currentAssembly = Assembly.GetExecutingAssembly();

        var types = currentAssembly.ExportedTypes.Where(x => x is { IsClass: true, IsPublic: true } && x.GetInterfaces().Any(y=> y == typeof(T))).ToList();

        foreach (var type in types)
        {
            service.AddSingleton(typeof(T), type);
        }
    }
}