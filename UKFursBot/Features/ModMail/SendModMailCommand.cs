using Discord;
using Discord.WebSocket;
using UKFursBot.Commands.CommandClassAttributes;
using UKFursBot.Context;

namespace UKFursBot.Features.ModMail;

[CommandName("modmail")]
[CommandDescription("Send a message to the moderators.")]
public class SendModMailCommand : BaseCommand<SendModMailCommandParameters>
{
    private readonly UKFursBotDbContext _dbContext;
    private readonly DiscordSocketClient _socketClient;
    private readonly SocketMessageChannelManager _messageChannelManager;

    public SendModMailCommand(UKFursBotDbContext dbContext, DiscordSocketClient socketClient, SocketMessageChannelManager messageChannelManager)
    {
        _dbContext = dbContext;
        _socketClient = socketClient;
        _messageChannelManager = messageChannelManager;
    }
    protected override async Task Implementation(SocketSlashCommand socketSlashCommand, SendModMailCommandParameters commandParameters)
    {
        var guildId = socketSlashCommand.GuildId.GetValueOrDefault();
        var botConfig = _dbContext.BotConfigurations.First(x => x.GuildId == guildId);

        if (botConfig.ModMailChannel == 0)
        {
            await _messageChannelManager.SendLoggingWarningMessageAsync("Modmail channel has not been set.");
            return;
        }

        var modMailChannel = await _socketClient.GetTextChannelAsync(botConfig.ModMailChannel);

        var content = new RichTextBuilder()
            .AddHeading2($"Message from {socketSlashCommand.User.Username} | <@{socketSlashCommand.User.Id}>")
            .AddText(commandParameters.Message)
            .Build();

        var embed = new EmbedBuilder()
        {
            Color = Color.Green,
            Description = content,
        }.Build();
        await modMailChannel.SendMessageAsync(embed: embed);
    }
}

public class SendModMailCommandParameters       
{
    [CommandParameterRequired]
    public string Message { get; set; }
}