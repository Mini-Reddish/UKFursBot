using Discord;
using Discord.WebSocket;
using UKFursBot.Commands;
using UKFursBot.Context;

namespace UKFursBot.Features.ModMail;
public class SendModMailCommand(
    UKFursBotDbContext dbContext,
    DiscordSocketClient socketClient,
    SocketMessageChannelManager socketMessageChannelManager)
    : BaseCommand<SendModMailCommandParameters>(socketMessageChannelManager)
{
    public override string CommandName => "modmail";
    public override string CommandDescription => "Send a message to the moderators.";

    protected override async Task Implementation(SocketSlashCommand socketSlashCommand, SendModMailCommandParameters commandParameters)
    {
        var botConfig = dbContext.BotConfigurations.First();

        if (botConfig.ModMailChannel == 0)
        {
            await socketMessageChannelManager.SendLoggingWarningMessageAsync("Modmail channel has not been set.");
            return;
        }

        var modMailChannel = await socketClient.GetTextChannelAsync(botConfig.ModMailChannel);

        var content = new RichTextBuilder()
            .AddHeading2($"Message from {socketSlashCommand.User.Username} | <@{socketSlashCommand.User.Id}>")
            .AddText(commandParameters.Message)
            .Build();

        var embed = new EmbedBuilder()
        {
            Color = Color.Green,
            Description = content,
        }.Build();
        if (modMailChannel != null)
        {
            await modMailChannel.SendMessageAsync(embed: embed);
        }
        else
        {
            await socketMessageChannelManager.SendLoggingWarningMessageAsync("Modmail channel is not valid.  A user attempted to communicate using the modmail command.");
        }
    }
}

public class SendModMailCommandParameters       
{
    [CommandParameterRequired]
    public required string Message { get; set; }
}