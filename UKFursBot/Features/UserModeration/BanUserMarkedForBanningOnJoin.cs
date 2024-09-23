using Discord;
using Discord.WebSocket;
using UKFursBot.Context;

namespace UKFursBot.Features.UserModeration;

public class BanUserMarkedForBanningOnJoin: IUserJoinedHandler
{
    private readonly UKFursBotDbContext _dbContext;
    private readonly DiscordSocketClient _client;

    public BanUserMarkedForBanningOnJoin(UKFursBotDbContext dbContext,  DiscordSocketClient client)
    {
        _dbContext = dbContext;
        _client = client;
    }
    public async Task HandleUserJoined(SocketGuildUser socketGuildUser)
    {
        var settings = _dbContext.BotConfigurations.FirstOrDefault();
        if (settings == null) 
            return;
        
        var userId = socketGuildUser.Id;

        if (_dbContext.BansOnJoin.All(x => x.UserId != userId))
            return;
        
        var response = new RichTextBuilder()
            .AddHeading2("User Banned On Join")
            .AddText($"User {socketGuildUser.Username} was marked for banning on join");
        
        var embed = new EmbedBuilder()
        {
            Color = Color.Red,
            Description = response.Build()
        }.Build();
        
        var channel = await _client.GetChannelAsync(settings.ModerationLoggingChannel);
        if (channel is SocketTextChannel textChannel)
        {
            await textChannel.SendMessageAsync(embed: embed);
        }
        
        await socketGuildUser.Guild.AddBanAsync(socketGuildUser);
    }
}