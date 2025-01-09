using Discord.WebSocket;
using UKFursBot.Commands;

namespace UKFursBot.Features.UserModeration;

public class BanByIdCommand(SocketMessageChannelManager socketMessageChannelManager, DiscordSocketClient socketClient, IBanUserAction banUserAction) : BaseCommand<BanByIdCommandParameters>(socketMessageChannelManager)
{ 
    public override string CommandName => "ban_user_by_id";
    public override string CommandDescription => "Ban a user by id.  Immediately or the next time they join if they have already left.";
    protected override async Task Implementation(SocketSlashCommand socketSlashCommand, BanByIdCommandParameters commandParameters)
    {
        var user = socketClient.GetGuild(socketSlashCommand.GuildId ?? 0)?.GetUser(commandParameters.UserId);

        if (user is null)
        {
            await FollowupAsync($"User {commandParameters.UserId} could not be found.");
            return;
        }
        await banUserAction.TryBanUser(user, socketSlashCommand.User, commandParameters.BanMessage,
            commandParameters.PurgeRecentMessages, commandParameters.SilentBan);
    }
}

public class BanByIdCommandParameters 
{
    
    [CommandParameterRequired]
    public ulong UserId { get; set; }
    
    [CommandParameterRequired]
    public required string BanMessage { get; set; }
    
    [CommandParameterRequired]
    public bool PurgeRecentMessages { get; set; }
    
    public bool SilentBan { get; set; }
}