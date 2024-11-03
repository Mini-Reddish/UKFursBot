using Discord.WebSocket;

namespace UKFursBot;

public interface IUserJoinedHandler
{
    Task HandleUserJoined(SocketGuildUser socketGuildUser);
}