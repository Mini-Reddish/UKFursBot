using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using UKFursBot.Commands;
using UKFursBot.Context;

namespace UKFursBot.Features.Posts;
public class DeletePostCommand : BaseCommand<DeletePostCommandParameters>
{
    private readonly UKFursBotDbContext _dbContext;
    private readonly SocketMessageChannelManager _socketMessageChannelManager;

    public DeletePostCommand(UKFursBotDbContext dbContext,SocketMessageChannelManager socketMessageChannelManager)
    {       
        _dbContext = dbContext;
        _socketMessageChannelManager = socketMessageChannelManager;
    }

    public override string CommandName => "delete_post";
    public override string CommandDescription => "Deletes the post with the given Post ID";

    protected override async Task Implementation(SocketSlashCommand socketSlashCommand, DeletePostCommandParameters commandParameters)
    {
        var createdPostForDeleting =
            await _dbContext.CreatePosts.SingleOrDefaultAsync(x => x.MessageId == commandParameters.PostId);
        if (createdPostForDeleting == null)
        {
            await FollowupAsync("Post not found in Database");
            return;
        }
        var messageChannel = await _socketMessageChannelManager.GetTextChannelFromIdAsync(createdPostForDeleting.ChannelId);
        if (messageChannel == null)
        {
            await _socketMessageChannelManager.SendLoggingErrorMessageAsync($"Could not find message channel with ID: {createdPostForDeleting.ChannelId}.  Removing from the database...");
            _dbContext.CreatePosts.Remove(createdPostForDeleting);
            return;
        }

        await messageChannel.DeleteMessageAsync(commandParameters.PostId);
        
    }
}

public class DeletePostCommandParameters   
{
    [CommandParameterRequired]
    [CommandParameterDescription("The ID of the message to be deleted")]
    public ulong PostId { get; set; }
}