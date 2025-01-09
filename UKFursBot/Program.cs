using System.Windows.Input;
using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UKFursBot.Commands;
using UKFursBot.Context;
using UKFursBot.Entities;
using UKFursBot.HandlerInterfaces;
using UKFursBot.SlashCommandParameterOptionStrategies;

namespace UKFursBot;

class Program
{
    private static IServiceProvider _services = null!;
    private BotGuildUsers _botGuildUsers = null!;

    public static int Main(string[] args)
    {
        _services = ServiceProvider.Instance;

        InitialiseDatabase();

        new Program().MainAsync().GetAwaiter().GetResult();
        return 0;
    }

    private static void InitialiseDatabase()
    {
        var dbContext = _services.GetRequiredService<UKFursBotDbContext>();
        dbContext.Database.Migrate();
    }

    private async Task MainAsync()
    {
        var config = _services.GetRequiredService<IConfiguration>();
        var client = _services.GetRequiredService<DiscordSocketClient>();
        _botGuildUsers = _services.GetRequiredService<BotGuildUsers>();
        var commandManager = _services.GetRequiredService<ICommandManager>();
        var commands = _services.GetServices<ISlashCommand>();
        
        foreach (var slashCommand in commands)
        {
            commandManager.AddSlashCommand(slashCommand);
        }
        

        client.Log += LogAsync;
        try
        {
            await client.LoginAsync(TokenType.Bot, config["AuthToken"]);
            await client.StartAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }

        client.Disconnected += ClientOnDisconnected;
        client.GuildAvailable += ClientOnGuildAvailable;
        client.SlashCommandExecuted += ClientOnSlashCommandExecuted;
        client.ModalSubmitted += ClientOnModalSubmitted;
        client.UserJoined += ClientOnUserJoined;
        client.MessageUpdated += ClientOnMessageUpdated;
        client.UserVoiceStateUpdated += ClientOnUserVoiceChannelChanged;
        client.MessageReceived += ClientOnMessageReceived;
        
        await Task.Delay(-1);
    }

    private Task ClientOnDisconnected(Exception arg)
    {
        Console.WriteLine(arg.Message);
        Console.WriteLine(arg.InnerException?.Message);
        return Task.CompletedTask;
    }

    private async Task ClientOnMessageReceived(SocketMessage message)
    {
        var dbContext = _services.GetRequiredService<UKFursBotDbContext>();
        
        if (message is not SocketUserMessage socketUserMessage)
            return;
        if (socketUserMessage.Type == MessageType.Default)
        {
            var userMessageReceivedHandlers = _services.GetServices<IUserMessageReceivedHandler>();
            foreach (var userMessageReceivedHandler in userMessageReceivedHandlers)
            {
                await userMessageReceivedHandler.HandleMessageReceived(socketUserMessage);
            }
        }
        else if (socketUserMessage is { Type: MessageType.Reply, ReferencedMessage: not null })
        {
            var userReplyMessageReceivedHandlers = _services.GetServices<IUserReplyMessageReceivedHandler>();
            foreach (var userReplyMessageReceivedHandler in userReplyMessageReceivedHandlers)
            {
                await userReplyMessageReceivedHandler.HandleMessageReceived(socketUserMessage, socketUserMessage.ReferencedMessage);
            }
        }

        await dbContext.SaveChangesAsync();
    }

    private async Task ClientOnUserVoiceChannelChanged(SocketUser user, SocketVoiceState previousVoiceState, SocketVoiceState currentVoiceState)
    {
        var dbContext = _services.GetRequiredService<UKFursBotDbContext>();
        var userVoiceChannelChangedHandlers = _services.GetServices<IUserVoiceChannelChangedHandler>();

        foreach (var userVoiceChannelChangedHandler in userVoiceChannelChangedHandlers)
        {
            await userVoiceChannelChangedHandler.HandleUserVoiceChannelChanged(user, previousVoiceState, currentVoiceState);
        }
        await dbContext.SaveChangesAsync();
    }

    private async Task ClientOnMessageUpdated(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel)
    {
        var dbContext = _services.GetRequiredService<UKFursBotDbContext>();
        var messageUpdatedHandlers = _services.GetServices<IMessageEditedHandler>();

        foreach (var messageUpdatedHandler in messageUpdatedHandlers)
        {
            await messageUpdatedHandler.HandleMessageUpdated(before, after, channel);
        }
        await dbContext.SaveChangesAsync();
    }

    private Task LogAsync(LogMessage arg)
    {
        Console.WriteLine(arg.Message);
        return Task.CompletedTask;
    }

    private async Task ClientOnUserJoined(SocketGuildUser arg)
    {
        var dbContext = _services.GetRequiredService<UKFursBotDbContext>();
        var onUserJoinedHandlers = _services.GetServices<IUserJoinedHandler>();
        
        foreach (var userJoinedHandler in onUserJoinedHandlers)
        {
            await userJoinedHandler.HandleUserJoined(arg);
        }
        await dbContext.SaveChangesAsync();
    }

    private async Task ClientOnModalSubmitted(SocketModal arg)
    {
        Console.WriteLine("modal submitted: " + arg.Data.CustomId);
        await arg.RespondAsync("done");
    }

    private async Task ClientOnSlashCommandExecuted(SocketSlashCommand arg)
    {
        var dbContext = _services.GetRequiredService<UKFursBotDbContext>();
        var commandManager = _services.GetRequiredService<ICommandManager>();
        var command = commandManager.TryGetSlashCommand(arg.CommandName);
        
        if (command == null)
        {
            return;
        }
        var commandParameters = arg.Data.MapDataToType(command.GetType().BaseType!.GetGenericArguments().First());

        var methodInfo = command.GetType().GetMethod("Execute");
        if (methodInfo != null)
        {
            await (Task) methodInfo.Invoke(command, new[] { arg, commandParameters })!;
        }

        await dbContext.SaveChangesAsync();
    }

    private async Task ClientOnGuildAvailable(SocketGuild guild)
    {
        await InitialiseGuildBotConfigurationInDb(guild);

        _botGuildUsers.TryAdd(guild.Id, guild.CurrentUser);
        var commandManager = _services.GetRequiredService<ICommandManager>();
        var guildCommands = new List<ApplicationCommandProperties>();
        foreach (var slashCommand in commandManager.GetAllSlashCommands())
        {
            var commandName = slashCommand.CommandName;
            var commandDescription = slashCommand.CommandDescription;
            try
            {
                var guildCommand = new SlashCommandBuilder()
                    .WithName(commandName.ToLowerInvariant())
                    .WithDescription(commandDescription)
                    .BuildOptionsFromParameters(slashCommand.GetType(), _services.GetRequiredService<SlashCommandParameterOptionStrategyResolver>())
                    .WithDefaultPermission(false)
                    .Build();
                
                guildCommands.Add(guildCommand);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error creating command {commandName}");
                Console.WriteLine(e);
            }
        }
        
        await guild.BulkOverwriteApplicationCommandAsync(guildCommands.ToArray());
    }

    private static async Task InitialiseGuildBotConfigurationInDb(SocketGuild guild)
    {
        var dbContext = _services.GetRequiredService<UKFursBotDbContext>();
        var guildBotConfiguration = dbContext.BotConfigurations.FirstOrDefault();
        if (guildBotConfiguration == null)
        {
            dbContext.BotConfigurations.Add(new BotConfiguration()
            {
                GuildId = guild.Id,
            });
            await dbContext.SaveChangesAsync();
        }
    }
}