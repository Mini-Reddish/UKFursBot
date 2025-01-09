using Discord;
using Discord.WebSocket;
using UKFursBot.Context;
using UKFursBot.Entities;

namespace UKFursBot.Features.UserModeration;

public class BanUserAction(UKFursBotDbContext dbContext, SocketMessageChannelManager socketMessageChannelManager) : IBanUserAction
{
    public async Task TryBanUser(SocketGuildUser user, SocketUser moderator,  string banMessage, bool purgeMessages, bool silentBan)
    {
        var sentToUser = false;
        if (!silentBan)
        {
            try
            {
                var dmChannel = await user.CreateDMChannelAsync();
                await dmChannel.SendMessageAsync(banMessage);
                sentToUser = true;
            }
            catch (Exception e)
            {
                await socketMessageChannelManager.SendLoggingErrorMessageAsync("Unable to DM User for ban", e);
            }
        }

        try
        {
            var prunePeriod = purgeMessages ? 7 : 0;
            await user.BanAsync(prunePeriod, banMessage);
        }
        catch (Exception e)
        {
            await socketMessageChannelManager.SendLoggingErrorMessageAsync("Unable to Ban User.  Adding them to ban on join", e);
            dbContext.BansOnJoin.Add(new BanOnJoin()
            {
                UserId = user.Id,
                ModeratorId = moderator.Id,
                Reason = banMessage
            });
            return;
        }

        dbContext.BanLogs.Add(new BanLog()
        {
            DateAdded = DateTime.UtcNow,
            UserId = user.Id,
            ModeratorId = user.Id,
            Reason = banMessage,
            WasSentToUser = sentToUser
        });
        
        var content = new RichTextBuilder()
            .AddHeading2("Banned User")
            .AddText($"<@{user.Id}> | {user.Username} has been banned")
            .AddHeading3("User ID")
            .AddHeading3("Message sent to user")
            .AddText(banMessage)
            .AddHeading3("User ID")
            .AddText(user.Id.ToString())
            .AddHeading3("Moderator")
            .AddText($"<@{moderator.Id}> | {moderator.Username}")
            .AddHeading3("Was User available for Direct messaging")
            .AddText(sentToUser ? "Yes" : "No").Build();

        var embed = new EmbedBuilder()
        {
            Color = Color.Orange,
            Description = content
        }.Build();

        await socketMessageChannelManager.SendModerationLoggingMessageAsync(embed);
    }
}