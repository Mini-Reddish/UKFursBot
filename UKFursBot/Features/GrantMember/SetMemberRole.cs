using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using UKFursBot.Commands;
using UKFursBot.Context;

namespace UKFursBot.Features.GrantMember;

public class SetMemberRoleCommand(UKFursBotDbContext dbContext, SocketMessageChannelManager socketMessageChannelManager)
    : BaseCommand<SetMemberRoleCommandArgs>(socketMessageChannelManager)
{
    public override string CommandName => "set_member_role";
    public override string CommandDescription => "Set the role assigned when granting a user membership";

    protected override async Task Implementation(SocketSlashCommand socketSlashCommand, SetMemberRoleCommandArgs commandParameters)
    {
        var config = await dbContext.BotConfigurations.FirstAsync();

        config.MemberRoleId = commandParameters.Role.Id;
    }
}

public class SetMemberRoleCommandArgs
{
    [CommandParameterDescription("The role we want to assign")]
    [CommandParameterRequired]
    public SocketRole Role { get; set; }
}