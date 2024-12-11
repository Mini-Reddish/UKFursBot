using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using UKFursBot.Commands;
using UKFursBot.Context;

namespace UKFursBot.Features.GrantMember;

public class SetMemberWelcomeMessageCommand : BaseCommand<SetMemberWelcomeMessageCommandParameters>
{
    private readonly UKFursBotDbContext _dbContext;
    public override string CommandName => "set_Member_welcome_message";

    public override string CommandDescription => "Sets the welcome message when granting the member role to a user.";

    public SetMemberWelcomeMessageCommand(UKFursBotDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    protected override async Task Implementation(SocketSlashCommand socketSlashCommand, SetMemberWelcomeMessageCommandParameters commandParameters)
    {
        var configuration = await _dbContext.BotConfigurations.FirstAsync();

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