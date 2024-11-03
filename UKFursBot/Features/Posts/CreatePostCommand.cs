
using Discord;
using Discord.WebSocket;
using UKFursBot.Commands;
using UKFursBot.Context;
using UKFursBot.Entities;

namespace UKFursBot.Features.Posts;

[CommandName("create_post")]
[CommandDescription("Starts the process for creating a new post.")]
public class CreatePostCommand : BaseCommand<CreatePostCommandParameters>
{
    private readonly UKFursBotDbContext _dbContext;
    private readonly SocketMessageChannelManager _socketMessageChannelManager;
    protected override bool IsEphemeral => false;

    public CreatePostCommand(UKFursBotDbContext dbContext,SocketMessageChannelManager socketMessageChannelManager)
    {
        _dbContext = dbContext;
        _socketMessageChannelManager = socketMessageChannelManager;
    }
    protected override async Task Implementation(SocketSlashCommand socketSlashCommand, CreatePostCommandParameters commandParameters)
    {
        var followUpMessage = await FollowupAsync($"Starting the process for creating a new post.  Reply to this message with the content of the message you want placed in <#{commandParameters.Channel.Id}>");
        if (followUpMessage == null)
        {
            await _socketMessageChannelManager.SendLoggingErrorMessageAsync("Failed to initiate the create post command.");   
            return;
        }

        _dbContext.CreatePosts.Add(new CreatePost()
        {
            CreatePostResponseToken = followUpMessage.Id,
            State = CreatePostState.Creating,
            ActivelyListenForResponseUntil = DateTime.UtcNow.AddMinutes(20),
            ChannelId = commandParameters.Channel.Id,
            UseEmbed = commandParameters.UseEmbed,
            EmbedColour = commandParameters.EmbedColour,
        });
    }
}

public class CreatePostCommandParameters
{

    [CommandParameterRequired]
    [CommandParameterDescription("The channel to create the post in")]
    public IGuildChannel Channel { get; set; } = null!;

    [CommandParameterRequired]
    [CommandParameterDescription("Do you want the content of your message to be wrapped inside an embedded component?")]
    public bool UseEmbed {get;set;}
    
    [CommandParameterDescription("If using an embed, what colour should the bar be at the side?")]
    public Color EmbedColour {get;set;}
}
