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
        if (socketUserMessage.Channel is not SocketDMChannel dmChannel)
            return;

        if (socketUserMessage.Author.MutualGuilds.Count == 1)
        {
            var guild = socketUserMessage.Author.MutualGuilds.First();

            var botConfig = _dbContext.BotConfigurations.First(x => x.GuildId == guild.Id);

            if (botConfig.ModMailChannel == 0)
            {
                await _messageChannelManager.SendLoggingWarningMessageAsync("Modmail channel has not been set.");
                return;
            }

            var modMailChannel = await _socketClient.GetTextChannelAsync(botConfig.ModMailChannel);

            var content = new RichTextBuilder()
                .AddHeading2($"Message from {socketUserMessage.Author.Username} | <@{socketUserMessage.Id}>")
                .AddText(socketUserMessage.Content)
                .Build();

            var embed = new EmbedBuilder()
            {
                Color = Color.Green,
                Description = content,
            }.Build();
            await modMailChannel.SendMessageAsync(embed: embed);
            await dmChannel.SendMessageAsync("Modmail sent, a moderator will be with you shortly to assist");
        }
        else
        {
            //TODO:  Respond asking for user context?
        }


    }
}