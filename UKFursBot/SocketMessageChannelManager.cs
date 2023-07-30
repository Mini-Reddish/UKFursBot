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
    
    public async Task SendLoggingWarningMessageAsync(string message)
    {
        var configuration = _dbContext.BotConfigurations.First();

        if (await _socketClient.GetChannelAsync(configuration.ErrorLoggingChannelId) is not ITextChannel loggingChannel)
            return; 

        var messageContents = new RichTextBuilder()
            .AddHeading1("Warning!")
            .AddText(message);
        var embedBuilder = new EmbedBuilder()
        {
            Description = messageContents.Build(),
            Color = Color.Orange,
            
        };

        await loggingChannel.SendMessageAsync(string.Empty, false, embedBuilder.Build());
    }
    
    public async Task SendLoggingErrorMessageAsync(string message, Exception exception)
    {
        var configuration = _dbContext.BotConfigurations.First();
        
        if (await _socketClient.GetChannelAsync(configuration.ErrorLoggingChannelId) is not ITextChannel loggingChannel)
            return; 

        var messageContents = new RichTextBuilder()
            .AddHeading1("Warning!")
            .AddText(message)
            .AddHeading2("Exception:")
            .AddText(exception.Message);
        
        var embedBuilder = new EmbedBuilder()
        {
            Description = messageContents.Build(),
            Color = Color.Red,
            
        };

        await loggingChannel.SendMessageAsync(string.Empty, false, embedBuilder.Build());
    }

    public async Task SendModerationLoggingMessageAsync(Embed embed)
    {
        var configuration = _dbContext.BotConfigurations.First();

        if (await _socketClient.GetChannelAsync(configuration.ModerationLoggingChannel) is not ITextChannel
            loggingChannel)
        {
            await SendLoggingWarningMessageAsync("Moderation Logging channel is not set");
            return;
        }

        await loggingChannel.SendMessageAsync(embed: embed);
    }
}