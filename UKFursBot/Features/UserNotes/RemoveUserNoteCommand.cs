using Discord;
using Discord.WebSocket;
using UKFursBot.Commands;
using UKFursBot.Context;

namespace UKFursBot.Features.UserNotes;
public class RemoveUserNoteCommand(
    UKFursBotDbContext dbContext,
    DiscordSocketClient client,
    SocketMessageChannelManager socketMessageChannelManager)
    : BaseCommand<RemoveUserNoteCommandParameters>(socketMessageChannelManager)
{
    public override string CommandName => "remove_note";
    public override string CommandDescription => "Removes the specified note ID from the specified user.";

    protected override async Task Implementation(SocketSlashCommand socketSlashCommand, RemoveUserNoteCommandParameters commandParameters)
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

        var userNote = dbContext.UserNotes.FirstOrDefault(un => un.UserId == userId && un.Id == commandParameters.NoteId);
        var user = await client.GetUserAsync(userId);
        
        if (userNote == null)
        {
            await FollowupAsync($"WARNING: The specified user note id {commandParameters.NoteId} could not be found for user: {user.Username}");
            return;
        }

        dbContext.UserNotes.Remove(userNote);
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
        
        var channel = await client.GetChannelAsync(settings.ModerationLoggingChannel);
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
    public ulong NoteId { get; set; }
}