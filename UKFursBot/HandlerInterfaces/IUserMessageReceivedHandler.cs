using Discord.WebSocket;

namespace UKFursBot;

public interface IUserMessageReceivedHandler
{
    Task HandleMessageReceived(SocketUserMessage socketMessage);
}