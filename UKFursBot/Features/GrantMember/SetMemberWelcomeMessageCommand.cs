using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using UKFursBot.Commands;
using UKFursBot.Context;

namespace UKFursBot.Features.GrantMember;

public class SetMemberWelcomeMessageCommand(
    UKFursBotDbContext dbContext,
    SocketMessageChannelManager socketMessageChannelManager)
    : BaseCommand<SetMemberWelcomeMessageCommandParameters>(socketMessageChannelManager)
{
    public override string CommandName => "set_Member_welcome_message";

    public override string CommandDescription => "Sets the welcome message when granting the member role to a user.";

    protected override async Task Implementation(SocketSlashCommand socketSlashCommand, SetMemberWelcomeMessageCommandParameters commandParameters)
    {
        var configuration = await dbContext.BotConfigurations.FirstAsync();

        configuration.MemberWelcomeMessage = commandParameters.WelcomeMessage;

        await FollowupAsync("Welcome message has been set.");
    }
}

public class SetMemberWelcomeMessageCommandParameters
{
    [CommandParameterDescription("The message to display in the chat when a user is granted the member role")]
    [CommandParameterRequired]
    public required string WelcomeMessage { get; set; }
}