using Discord;
using Discord.Rest;
using Discord.WebSocket;
using UKFursBot.Context;
using UKFursBot.HandlerInterfaces;

namespace UKFursBot.Features.Posts;

public class EditPostReplyMessageReceived  : IUserReplyMessageReceivedHandler
{
    private readonly UKFursBotDbContext _dbContext;
    private readonly BotGuildUsers _botGuildUsers;

    public EditPostReplyMessageReceived(UKFursBotDbContext dbContext, BotGuildUsers botGuildUsers)
    {
        _dbContext = dbContext;
        _botGuildUsers = botGuildUsers;
    }
    public async Task HandleMessageReceived(SocketUserMessage socketMessage, IUserMessage userMessageReplyingTo)
    {
        if (userMessageReplyingTo.Channel is not SocketTextChannel textChannel)
        {
            return;
        }

        if (_botGuildUsers[textChannel.Guild.Id].Id != userMessageReplyingTo.Author.Id)
        {
            return;
        }
        
        var referencedMessageId = userMessageReplyingTo.Id;
        
        var createPostsEntry = _dbContext.CreatePosts.SingleOrDefault(x => x.CreatePostResponseToken == referencedMessageId);

        if (createPostsEntry is null)
        {
            return;
        }

        if (createPostsEntry.State != CreatePostState.Editing)
        {
            return;
        }
        
        var guild = textChannel.Guild;

        if (guild.GetChannel(createPostsEntry.ChannelId) is not SocketTextChannel channel)
        {
            return;
        }
        
        var messageContents = socketMessage.Content;
        
        if (string.IsNullOrWhiteSpace(messageContents))
        {
            return;
        }
        IUserMessage restUserMessage;
        
        if (createPostsEntry.UseEmbed)
        {
            var embed = new EmbedBuilder()
            {
                Color = createPostsEntry.EmbedColour,
                Description = messageContents,
            }.Build();
            
            restUserMessage = await channel.ModifyMessageAsync(createPostsEntry.MessageId,x => x.Embed= embed);
        }
        else
        {
            restUserMessage = await channel.ModifyMessageAsync(createPostsEntry.MessageId, x => x.Content = messageContents);
        }

        createPostsEntry.State = CreatePostState.Created;
        createPostsEntry.MessageId = restUserMessage.Id;
        
    }
}