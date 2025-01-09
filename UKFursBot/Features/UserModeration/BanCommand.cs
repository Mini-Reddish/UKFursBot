using Discord;
using Discord.WebSocket;
using UKFursBot.Commands;
using UKFursBot.Context;
using UKFursBot.Entities;

namespace UKFursBot.Features.UserModeration;
public class BanCommand(SocketMessageChannelManager socketMessageChannelManager, IBanUserAction banUserAction)
    : BaseCommand<BanCommandParameters>(socketMessageChannelManager)
{
    public override string CommandName => "ban_user";

    public override string CommandDescription =>
        "Ban the specified user, immediately or the next time they join if they have already left.";

    protected override async Task Implementation(SocketSlashCommand socketSlashCommand, BanCommandParameters commandParameters)
    {
        await banUserAction.TryBanUser(commandParameters.User, socketSlashCommand.User, commandParameters.BanMessage,
            commandParameters.PurgeRecentMessages, commandParameters.SilentBan);
    }
}

public class BanCommandParameters   
{
    [CommandParameterRequired]
    public SocketGuildUser User { get; set; }
    
    [CommandParameterRequired]
    public required string BanMessage { get; set; }
    
    [CommandParameterRequired]
    public bool PurgeRecentMessages { get; set; }
    
    public bool SilentBan { get; set; }
}