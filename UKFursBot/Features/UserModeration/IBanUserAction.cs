using Discord.WebSocket;

namespace UKFursBot.Features.UserModeration;

public interface IBanUserAction
{
    Task TryBanUser(SocketGuildUser user, SocketUser moderator, string banMessage, bool purgeMessages,
        bool silentBan);
}