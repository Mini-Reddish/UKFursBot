using Discord.WebSocket;

namespace UKFursBot.UserJoined;

public interface IUserJoinedHandler
{
    Task HandleUserJoined(SocketGuildUser socketGuildUser);
}