using Discord.WebSocket;
using UKFursBot.Context;
using UKFursBot.Features.Configuration;

namespace UKFursBot.Features.UserGreeting;

public class LogUserJoined : IUserJoinedHandler
{
    private readonly UKFursBotDbContext _dbContext;
    private readonly SocketMessageChannelManager _socketMessageChannelManager;
    private readonly DiscordSocketClient _client;

    public LogUserJoined(UKFursBotDbContext dbContext, SocketMessageChannelManager socketMessageChannelManager, DiscordSocketClient client)
    {
        _dbContext = dbContext;
        _socketMessageChannelManager = socketMessageChannelManager;
        _client = client;
    }
    public async Task HandleUserJoined(SocketGuildUser socketGuildUser)
    {
        var botConfig = _dbContext.BotConfigurations.First();

        if (botConfig.UserJoinLoggingEnabled == false)
            return;
        
        var userJoinLogChannelId = botConfig.UserJoinLoggingChannelId;

        if (userJoinLogChannelId == 0)
        {
            await _socketMessageChannelManager.SendLoggingWarningMessageAsync(
                $"Unable to log user joining as there was no channel set to log to.  Please set the {nameof(AdminMessageTypes.UserJoinLog)} Admin setting to a valid channel");
            return;
        }

        var channel = await _client.GetTextChannelAsync(userJoinLogChannelId);

        if (channel == null)
        {
            await _socketMessageChannelManager.SendLoggingWarningMessageAsync(
                "Unable to log user joining, the channel set to receive logging is not considered a text channel");
            return;
        }

        await channel.SendMessageAsync($"The following user has joined! <@{socketGuildUser.Id}>");
    }
}