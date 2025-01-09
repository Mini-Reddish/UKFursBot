using Discord;
using Discord.WebSocket;
using UKFursBot.Commands;
using UKFursBot.Context;

namespace UKFursBot.Features.ModMail;

public class SetModMailResponseMessage(
    UKFursBotDbContext dbContext,
    SocketMessageChannelManager socketMessageChannelManager)
    : BaseCommand<SetModMailResponseMessageCommandParameters>(socketMessageChannelManager)
{
    public override string CommandName => "set_modmail_response";

    public override string CommandDescription => "Sets the response given to a user when they send in a modmail message";

    protected override async Task Implementation(SocketSlashCommand socketSlashCommand, SetModMailResponseMessageCommandParameters commandParameters)
    {
        var botConfig = dbContext.BotConfigurations.First();

        if (string.IsNullOrWhiteSpace(commandParameters.Message))
        {
            await FollowupAsync("Please specify a valid message");
            return;
        }

        botConfig.ModMailResponseMessage = commandParameters.Message;
        
        var content = new RichTextBuilder()
            .AddHeading1("Modmail received Response")
            .AddText($"I have set the Modmail received response message to {commandParameters.Message}")
            .AddText($"Action by <@{socketSlashCommand.User.Id}>");
        
        var embed = new EmbedBuilder()
        {
            Color = Color.Blue,
            Description = content.Build(),
        }.Build();
        
        await socketMessageChannelManager.SendModerationLoggingMessageAsync(embed);
    }
}

public class SetModMailResponseMessageCommandParameters
{
    [CommandParameterRequired]
    public required string Message { get; set; }
}