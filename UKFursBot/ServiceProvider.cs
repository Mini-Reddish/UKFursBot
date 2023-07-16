using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UKFursBot.Commands;
using UKFursBot.Context;

namespace UKFursBot;

public static class ServiceProvider
{
    private static IServiceProvider? _instance;
    public static IServiceProvider Instance => _instance ?? CreateServiceProvider();

    private static IServiceProvider CreateServiceProvider()
    {
        if (_instance == null)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: false)
                .AddEnvironmentVariables();
            
            IConfiguration configuration = builder.Build();
            var service = new ServiceCollection();
            service.AddSingleton(configuration);
            service.AddDbContext<UKFursBotDbContext>();
            service.AddSingletonOfType<ISlashCommand>();
            
            _instance = service.BuildServiceProvider();
        }

        return _instance;
    }
    
   
}

static class ServiceCollectionExtensions
{
    public static ServiceCollection AddSingletonOfType<T>(this ServiceCollection service)
    {
        var currentAssembly = Assembly.GetExecutingAssembly();

        var types = currentAssembly.ExportedTypes.Where(x => x is { IsClass: true, IsPublic: true } && x.GetInterfaces().Any(y=> y == typeof(T))).ToList();

        foreach (var type in types)
        {
            service.AddSingleton(typeof(T), type);
        }
        return service;
    }
}