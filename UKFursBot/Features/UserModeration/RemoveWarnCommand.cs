using Discord.WebSocket;
using UKFursBot.Commands;
using UKFursBot.Context;

namespace UKFursBot.Features.UserModeration;
public class RemoveWarnCommand(
    UKFursBotDbContext dbContext,
    DiscordSocketClient client,
    SocketMessageChannelManager socketMessageChannelManager)
    : BaseCommand<RemoveWarnCommandParameters>(socketMessageChannelManager)
{
    public override string CommandName => "remove_warn";
    public override string CommandDescription => "Removes the specified warning from the current user";

    protected override async Task Implementation(SocketSlashCommand socketSlashCommand, RemoveWarnCommandParameters commandParameters)
    {
        if (commandParameters.User == null && commandParameters.UserId == 0)
        {
            await FollowupAsync("A user, or user ID needs to be specified");
            return;
        }
        
        var userId = commandParameters.User?.Id ?? commandParameters.UserId;
        
        var settings = dbContext.BotConfigurations.First();
        
        if (settings.ModerationLoggingChannel == 0)
        {
            await FollowupAsync("WARNING: The logging channel has not been assigned.  Please assign one using the /admin_set_channel_for ModerationLog command");
            return;
        }

        var warning = dbContext.Warnings.FirstOrDefault(warning => warning.UserId == userId && warning.Id == commandParameters.WarningId);
        var user = await client.GetUserAsync(userId);
            
        if (warning == null)
        {
            await FollowupAsync($"WARNING: The specified warning id {commandParameters.WarningId} could not be found for user: {user.Username}");
            return;
        }
    }
}

public class RemoveWarnCommandParameters
{
    public SocketGuildUser? User { get; set; }

    public ulong UserId { get; set; }
    
    [CommandParameterRequired]
    public ulong WarningId { get; set; }
}