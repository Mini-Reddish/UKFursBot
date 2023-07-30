using System.Reflection;
using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UKFursBot.Commands.CommandClassAttributes;
using UKFursBot.Context;
using UKFursBot.Entities;

namespace UKFursBot;

class Program
{
    private static IServiceProvider _services = null!;
    private BotGuildUsers botGuildUsers;

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
        botGuildUsers = _services.GetRequiredService<BotGuildUsers>();

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

        client.GuildAvailable += ClientOnGuildAvailable;
        client.SlashCommandExecuted += ClientOnSlashCommandExecuted;
        client.ModalSubmitted += ClientOnModalSubmitted;
        client.UserJoined += ClientOnUserJoined;
        client.MessageUpdated += ClientOnMessageUpdated;
        client.UserVoiceStateUpdated += ClientOnUserVoiceChannelChanged;
        client.MessageReceived += ClientOnMessageReceived;

        await Task.Delay(-1);
    }

    private async Task ClientOnMessageReceived(SocketMessage message)
    {
        var userMessageReceivedHandlers = _services.GetServices<IUserMessageReceivedHandler>();

        if(message is SocketUserMessage socketUserMessage)
        foreach (var userMessageReceivedHandler in userMessageReceivedHandlers)
        {
            await userMessageReceivedHandler.HandleMessageReceived(socketUserMessage);
        }
    }

    private async Task ClientOnUserVoiceChannelChanged(SocketUser user, SocketVoiceState previousVoiceState, SocketVoiceState currentVoiceState)
    {
        var userVoiceChannelChangedHandlers = _services.GetServices<IUserVoiceChannelChangedHandler>();

        foreach (var userVoiceChannelChangedHandler in userVoiceChannelChangedHandlers)
        {
            await userVoiceChannelChangedHandler.HandleUserVoiceChannelChanged(user, previousVoiceState, currentVoiceState);
        }
    }

    private async Task ClientOnMessageUpdated(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel)
    {
        var messageUpdatedHandlers = _services.GetServices<IMessageEditedHandler>();

        foreach (var messageUpdatedHandler in messageUpdatedHandlers)
        {
            await messageUpdatedHandler.HandleMessageUpdated(before, after, channel);
        }
    }

    private Task LogAsync(LogMessage arg)
    {
        Console.WriteLine(arg.Message);
        return Task.CompletedTask;
    }

    private async Task ClientOnUserJoined(SocketGuildUser arg)
    {
        var onUserJoinedHandlers = _services.GetServices<IUserJoinedHandler>();
        
        foreach (var userJoinedHandler in onUserJoinedHandlers)
        {
            await userJoinedHandler.HandleUserJoined(arg);
        }
    }

    private async Task ClientOnModalSubmitted(SocketModal arg)
    {
        Console.WriteLine("modal submitted: " + arg.Data.CustomId);
        await arg.RespondAsync("done");
    }

    private async Task ClientOnSlashCommandExecuted(SocketSlashCommand arg)
    {
        var command = _services.GetServices<ISlashCommand>().FirstOrDefault(x =>
        {
            var commandNameAttribute = x.GetType().GetCustomAttribute<CommandNameAttribute>();
            return string.Equals(commandNameAttribute?.Name, arg.CommandName, StringComparison.InvariantCultureIgnoreCase);
        });
        
        if (command?.GetType().BaseType == null)
        {
            //Log error that command implementation does not exist.  Should never happen by design.
            return;
        }
        var commandParameters = arg.Data.MapDataToType(command.GetType().BaseType!.GetGenericArguments().First());

        var methodInfo = command.GetType().GetMethod("Execute");
        if (methodInfo != null)
        {
            await (Task) methodInfo.Invoke(command, new[] { arg, commandParameters })!;
        }


    }

    private async Task ClientOnGuildAvailable(SocketGuild guild)
    {
        await InitialiseGuildBotConfigurationInDb(guild);

        botGuildUsers.TryAdd(guild.Id, guild.CurrentUser);
        var commands = _services.GetServices<ISlashCommand>();
        var guildCommands = new List<ApplicationCommandProperties>();
        foreach (var slashCommand in commands)
        {
            try
            {
                var commandNameAttribute = slashCommand.GetType().GetCustomAttribute<CommandNameAttribute>();
                var commandDescriptionAttribute = slashCommand.GetType().GetCustomAttribute<CommandDescriptionAttribute>();

                if (commandNameAttribute == null || commandDescriptionAttribute == null)
                {
                    //TODO: Log missing command attributes.
                    continue;
                }
                var guildCommand = new SlashCommandBuilder()
                    .WithName(commandNameAttribute.Name.ToLowerInvariant())
                    .WithDescription(commandDescriptionAttribute.Description)
                    .BuildOptionsFromParameters(slashCommand)
                    .WithDefaultPermission(false)
                    .Build();
                
                guildCommands.Add(guildCommand);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        await guild.BulkOverwriteApplicationCommandAsync(guildCommands.ToArray());
    }

    private static async Task InitialiseGuildBotConfigurationInDb(SocketGuild guild)
    {
        var dbContext = _services.GetRequiredService<UKFursBotDbContext>();
        var guildBotConfiguration = dbContext.BotConfigurations.FirstOrDefault(x => x.GuildId == guild.Id);
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