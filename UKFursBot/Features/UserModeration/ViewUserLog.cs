using Discord;
using Discord.WebSocket;
using UKFursBot.Commands;
using UKFursBot.Context;
using UKFursBot.Entities;

namespace UKFursBot.Features.UserModeration;
public class ViewUserLogCommand : BaseCommand<ViewUserLogCommandParameters>
{
    private readonly UKFursBotDbContext _dbContext;
    private readonly DiscordSocketClient _socketClient;


    public ViewUserLogCommand(UKFursBotDbContext dbContext, DiscordSocketClient socketClient)
    {
        _dbContext = dbContext;
        _socketClient = socketClient;
    }

    public override string CommandName => "get_user_log";
    public override string CommandDescription => "Retrieves all Bans, warns, and user notes";

    protected override async Task Implementation(SocketSlashCommand socketSlashCommand, ViewUserLogCommandParameters commandParameters)
    {
        if (commandParameters.User == null && commandParameters.UserId == 0)
        {
            await FollowupAsync("Invalid parameters passed in.  Pass in either a user, or their user Id");
            return;
        }

        var guildSettings = _dbContext.BotConfigurations.First();
        var userId = commandParameters.User?.Id ?? commandParameters.UserId;

        var bans = _dbContext.BanLogs.Where(x => x.UserId == userId).ToList();
        var warnings = _dbContext.Warnings.Where(x => x.UserId == userId).ToList();
        var userNotes = _dbContext.UserNotes.Where(x => x.UserId == userId).ToList();
        var content = new RichTextBuilder()
            .AddHeading2($"<@{userId}>'s User Log");
        
        if (bans.Count > 0)
        {
            content.AddHeading3("Bans")
                .AddBulletedListItems<IEnumerable<BanLog>, BanLog>(bans, ban => ban.Reason);
        }

        if (warnings.Count > 0)
        {
            content.AddHeading3("Warnings")
                .AddBulletedListItems<IEnumerable<Warning>, Warning>(warnings, FormattedWarning);
        }

        if (userNotes.Count > 0)
        {
            content.AddHeading3("User Notes")
                .AddBulletedListItems<IEnumerable<UserNote>, UserNote>(userNotes, FormattedUserNote);
        }

        if (bans.Count == 0 && warnings.Count == 0 && userNotes.Count == 0)
        {
            content.AddText("This user has nothing.  They are a good bean :3");
        }

        var embed = new EmbedBuilder()
        {
            Color = Color.DarkPurple,
            Description = content.Build()
        }.Build();

        var moderationMessageChannel = await _socketClient.GetTextChannelAsync(guildSettings.ModerationLoggingChannel);
        if (moderationMessageChannel != null)
        {
            await moderationMessageChannel.SendMessageAsync($"<@{socketSlashCommand.User.Id}>", embed: embed);
        }
    }

    private string FormattedWarning(Warning warning)
    {
        return $"ID:{warning.Id} - {warning.Reason} - By:<@{warning.ModeratorId}> ({warning.DateAdded.ToShortDateString()})";
    }
    
    private string FormattedUserNote(UserNote userNote)
    {
        return $"ID:{userNote.Id} - {userNote.Reason} - By:<@{userNote.ModeratorId}> ({userNote.DateAdded.ToShortDateString()})";
    }
}

public class ViewUserLogCommandParameters       
{
    public SocketGuildUser? User { get; set; }
    
    public ulong UserId { get; set; }
}