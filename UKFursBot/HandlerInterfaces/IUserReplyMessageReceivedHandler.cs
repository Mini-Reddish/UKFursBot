using Discord;
using Discord.WebSocket;

namespace UKFursBot.HandlerInterfaces;

public interface IUserReplyMessageReceivedHandler
{
    Task HandleMessageReceived(SocketUserMessage socketMessage, IUserMessage userMessageReplyingTo);
}