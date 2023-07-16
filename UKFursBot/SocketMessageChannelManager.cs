using Discord;
using Discord.WebSocket;
using UKFursBot.Context;

namespace UKFursBot;

public class SocketMessageChannelManager 
{
    private readonly UKFursBotDbContext _dbContext;
    private readonly DiscordSocketClient _socketClient;

    public SocketMessageChannelManager(UKFursBotDbContext dbContext, DiscordSocketClient socketClient)
    {
        _dbContext = dbContext;
        _socketClient = socketClient;
    }
    public async Task SendLoggingErrorMessageAsync(string message, Exception exception)
    {
        var configuration = _dbContext.BotConfigurations.FirstOrDefault();
        if(configuration == null)
            return; //TODO: some other form of logging to inform people that it's not set!

        if (await _socketClient.GetChannelAsync(configuration.ErrorLoggingChannelId) is not ITextChannel loggingChannel)
            return; //TODO: some other form of logging to inform people that it's set to an incorrect type of channel.
        
        var embedBuilder = new EmbedBuilder()
        {
            Description = $"{message}, The exception was {exception.Message} ",
            Color = Color.Red
        };

        await loggingChannel.SendMessageAsync(string.Empty, false, embedBuilder.Build());
    }
}