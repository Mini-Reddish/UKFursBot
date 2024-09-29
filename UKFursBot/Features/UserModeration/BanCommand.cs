using Discord;
using Discord.WebSocket;
using UKFursBot.Commands;
using UKFursBot.Context;
using UKFursBot.Entities;

namespace UKFursBot.Features.UserModeration;

[CommandName("ban")]
[CommandDescription("Ban the specified user, immediately or the next time they join if they have already left.")]
public class BanCommand : BaseCommand<BanCommandParameters>
{
    private readonly SocketMessageChannelManager _socketMessageChannelManager;
    private readonly UKFursBotDbContext _dbContext;

    public BanCommand(SocketMessageChannelManager socketMessageChannelManager, UKFursBotDbContext dbContext)
    {
        _socketMessageChannelManager = socketMessageChannelManager;
        _dbContext = dbContext;
    }
    protected override async Task Implementation(SocketSlashCommand socketSlashCommand, BanCommandParameters commandParameters)
    {
        var sentToUser = false;
        try
        {
            var dmChannel = await commandParameters.User.CreateDMChannelAsync();
            await dmChannel.SendMessageAsync(commandParameters.BanMessage);
            sentToUser = true;
        }
        catch (Exception e)
        {
            await _socketMessageChannelManager.SendLoggingErrorMessageAsync("Unable to DM User for ban", e);
        }

        try
        {
            var prunePeriod = commandParameters.PurgeRecentMessages ? 7 : 0;
            await commandParameters.User.BanAsync(prunePeriod, commandParameters.BanMessage);
        }
        catch (Exception e)
        {
            await _socketMessageChannelManager.SendLoggingErrorMessageAsync("Unable to Ban User.  Adding them to ban on join", e);
            _dbContext.BansOnJoin.Add(new BanOnJoin()
            {
                UserId = commandParameters.User.Id,
                ModeratorId = socketSlashCommand.User.Id,
                Reason = commandParameters.BanMessage
            });
            return;
        }

        _dbContext.BanLogs.Add(new BanLog()
        {
            DateAdded = DateTime.UtcNow,
            UserId = commandParameters.User.Id,
            ModeratorId = socketSlashCommand.User.Id,
            Reason = commandParameters.BanMessage,
            WasSentToUser = sentToUser
        });
        
        var content = new RichTextBuilder()
            .AddHeading2("Banned User")
            .AddText($"<@{commandParameters.User.Id}> | {commandParameters.User.Username} has been banned")
            .AddHeading3("User ID")
            .AddHeading3("Message sent to user")
            .AddText(commandParameters.BanMessage)
            .AddHeading3("User ID")
            .AddText(commandParameters.User.Id.ToString())
            .AddHeading3("Moderator")
            .AddText($"<@{socketSlashCommand.User.Id}> | {socketSlashCommand.User.Username}")
            .AddHeading3("Was User available for Direct messaging")
            .AddText(sentToUser ? "Yes" : "No").Build();

        var embed = new EmbedBuilder()
        {
            Color = Color.Orange,
            Description = content
        }.Build();

        await _socketMessageChannelManager.SendModerationLoggingMessageAsync(embed);
    }
}

public class BanCommandParameters   
{
    [CommandParameterRequired]
    public required SocketGuildUser User { get; set; }
    
    
    [CommandParameterRequired]
    public required string BanMessage { get; set; }
    
    [CommandParameterRequired]
    public bool PurgeRecentMessages { get; set; }
}