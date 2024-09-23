using Discord;
using Discord.WebSocket;
using UKFursBot.Commands;
using UKFursBot.Context;

namespace UKFursBot.Features.UserNotes;

[CommandName("remove_note")]
[CommandDescription("Removes the specified note ID from the specified user.")]
public class RemoveUserNoteCommand  : BaseCommand<RemoveUserNoteCommandParameters>
{
    private readonly UKFursBotDbContext _dbContext;
    private readonly DiscordSocketClient _client;

    public RemoveUserNoteCommand(UKFursBotDbContext dbContext, DiscordSocketClient client)
    {
        _dbContext = dbContext;
        _client = client;
    }
    protected override async Task Implementation(SocketSlashCommand socketSlashCommand, RemoveUserNoteCommandParameters commandParameters)
    {
        if (commandParameters.User == null && commandParameters.UserId == 0)
        {
            await FollowupAsync("A user, or user ID needs to be specified");
            return;
        }

        var userId = commandParameters.User?.Id ?? commandParameters.UserId;
        
        var settings = _dbContext.BotConfigurations.FirstOrDefault();
        if (settings == null) 
            return;
        
        if (settings.ModerationLoggingChannel == 0)
        {
            await FollowupAsync("WARNING: The logging channel has not been assigned.  Please assign one using the /admin_set_channel_for ModerationLog command");
            return;
        }

        var userNote = _dbContext.UserNotes.FirstOrDefault(un => un.UserId == userId && un.Id == commandParameters.NoteId);
        var user = await _client.GetUserAsync(userId);
        
        if (userNote == null)
        {
            await FollowupAsync($"WARNING: The specified user note id {commandParameters.NoteId} could not be found for user: {user.Username}");
            return;
        }

        _dbContext.UserNotes.Remove(userNote);
        var response = new RichTextBuilder()
            .AddHeading2("User Note Removed")
            .AddText(userNote.Reason)
            .AddHeading3("Moderator")
            .AddText(socketSlashCommand.User.Username);

        var embed = new EmbedBuilder()
        {
            Color = Color.Orange,
            Description = response.Build()
        }.Build();
        
        var channel = await _client.GetChannelAsync(settings.ModerationLoggingChannel);
        if (channel is SocketTextChannel textChannel)
        {
            await textChannel.SendMessageAsync(embed: embed);
        }
        await FollowupAsync("Note removed");
    }
}

public class RemoveUserNoteCommandParameters
{
    public SocketGuildUser? User { get; set; }

    public ulong UserId { get; set; }
    
    [CommandParameterRequired]
    public int NoteId { get; set; }
}