using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using UKFursBot.Commands;
using UKFursBot.Context;

namespace UKFursBot.Features.GrantMember;

public class GrantMemberCommand : BaseCommand<GrantMemberCommandParameters>
{
    private readonly UKFursBotDbContext _dbContext;
    private readonly SocketMessageChannelManager _socketMessageChannelManager;

    public GrantMemberCommand(UKFursBotDbContext dbContext, SocketMessageChannelManager socketMessageChannelManager)
    {
        _dbContext = dbContext;
        _socketMessageChannelManager = socketMessageChannelManager;
    }

    public override string CommandName => "member";
    public override string CommandDescription => "Grant the specified user the member role";
    
    protected override async Task Implementation(SocketSlashCommand socketSlashCommand, GrantMemberCommandParameters commandParameters)
    {
        var config = await _dbContext.BotConfigurations.FirstAsync();
        var memberRoleId = config.MemberRoleId;
        if (socketSlashCommand.Channel is not SocketGuildChannel socketGuildChannel)
            return;

        if (memberRoleId is 0)
        {
            await _socketMessageChannelManager.SendLoggingErrorMessageAsync($"No role has been configured.");
            await FollowupAsync("I failed to grant the role, sorry");
            return;
        }
        var guild = socketGuildChannel.Guild;
        var memberRole = await guild.GetRoleAsync(memberRoleId);

        if (memberRole is null)
        {
            await _socketMessageChannelManager.SendLoggingErrorMessageAsync($"No role was found with the id of {memberRoleId}");
            await FollowupAsync("I failed to grant the role, sorry");
            return;
        }

        if (commandParameters.User.Roles.Any(r => r.Id == memberRole.Id))
        {
            await FollowupAsync("User is already a member!  >:c");
            return;
        }
        await commandParameters.User.AddRoleAsync(memberRole);
        
        var memberWelcomeChannelId = config.MemberWelcomeChannelId;
        var channel = guild.GetTextChannel(memberWelcomeChannelId);

        if (channel is null)
        {
            await _socketMessageChannelManager.SendLoggingErrorMessageAsync($"No channel was found with the id of {memberWelcomeChannelId}");
            await FollowupAsync("User was granted the role, but it was not announced.");
            return;
        }

        var welcomeMessage = config.MemberWelcomeMessage;

        if (welcomeMessage is null or "")
        {
            await _socketMessageChannelManager.SendLoggingErrorMessageAsync("No welcome message was defined for granting a new user the member role");
            await FollowupAsync("User was granted the role, but it was not announced.");
            return;
        }

        var formattedWelcomeMessage = FormatWelcomeMessage(welcomeMessage, commandParameters.User);
        await channel.SendMessageAsync(formattedWelcomeMessage);
        
    }

    private string FormatWelcomeMessage(string welcomeMessage, SocketGuildUser user)
    {
        return welcomeMessage.Replace("{user}", $"<@{user.Id}>");
    }
}

public class GrantMemberCommandParameters 
{

    [CommandParameterRequired]
    public required SocketGuildUser User { get; set; } = null!;
}