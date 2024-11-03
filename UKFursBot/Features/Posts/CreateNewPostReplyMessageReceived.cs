using Discord;
using Discord.Rest;
using Discord.WebSocket;
using UKFursBot.Context;
using UKFursBot.HandlerInterfaces;

namespace UKFursBot.Features.Posts;

public class CreateNewPostReplyMessageReceived : IUserReplyMessageReceivedHandler
{
    private readonly UKFursBotDbContext _dbContext;
    private readonly BotGuildUsers _botGuildUsers;

    public CreateNewPostReplyMessageReceived(UKFursBotDbContext dbContext, BotGuildUsers botGuildUsers)
    {
        _dbContext = dbContext;
        _botGuildUsers = botGuildUsers;
    }
    public async Task HandleMessageReceived(SocketUserMessage socketMessage,IUserMessage userMessageReplyingTo)
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

        if (createPostsEntry.State != CreatePostState.Creating)
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

        RestUserMessage restUserMessage;
        
        if (createPostsEntry.UseEmbed)
        {
            var embed = new EmbedBuilder()
            {
                Color = createPostsEntry.EmbedColour,
                Description = messageContents,
            }.Build();
            
            restUserMessage = await channel.SendMessageAsync(embed: embed);
        }
        else
        {
            restUserMessage = await channel.SendMessageAsync(messageContents);
        }

        createPostsEntry.State = CreatePostState.Created;
        createPostsEntry.MessageId = restUserMessage.Id;

    }
}
