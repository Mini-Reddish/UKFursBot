using Discord;
using Discord.WebSocket;
using UKFursBot.Context;

namespace UKFursBot.Features.ModMail;

public class OnDmHandleModMail : IUserMessageReceivedHandler
{
    private readonly UKFursBotDbContext _dbContext;
    private readonly SocketMessageChannelManager _messageChannelManager;
    private readonly DiscordSocketClient _socketClient;

    public OnDmHandleModMail(UKFursBotDbContext dbContext, SocketMessageChannelManager messageChannelManager, DiscordSocketClient socketClient)
    {
        _dbContext = dbContext;
        _messageChannelManager = messageChannelManager;
        _socketClient = socketClient;
    }

    public async Task HandleMessageReceived(SocketUserMessage socketUserMessage)
    {
        if (socketUserMessage.Author.Id == _socketClient.CurrentUser.Id)
            return;
        
        if (socketUserMessage.Channel is not SocketDMChannel dmChannel)
            return;

        var botConfig = _dbContext.BotConfigurations.First();

        if (botConfig.ModMailChannel == 0)
        {
            await _messageChannelManager.SendLoggingWarningMessageAsync("Modmail channel has not been set.");
            return;
        }

        if (string.IsNullOrWhiteSpace(botConfig.ModMailResponseMessage))
        {
            return;
        }
        
        var content = new RichTextBuilder()
            .AddHeading2($"Message from {socketUserMessage.Author.Username} | <@{socketUserMessage.Id}>")
            .AddText(socketUserMessage.Content)
            .Build();

        var embed = new EmbedBuilder()
        {
            Color = Color.Green,
            Description = content,
        }.Build();
        
        var modMailChannel = await _socketClient.GetTextChannelAsync(botConfig.ModMailChannel);
        if (modMailChannel != null)
        {
            await modMailChannel.SendMessageAsync(embed: embed);
        }
        else
        {
            await _messageChannelManager.SendLoggingWarningMessageAsync("Modmail channel specified was not a valid text channel.");
        }
        await dmChannel.SendMessageAsync(botConfig.ModMailResponseMessage);
    }
}