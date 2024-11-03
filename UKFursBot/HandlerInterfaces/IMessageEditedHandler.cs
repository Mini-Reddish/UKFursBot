using Discord;
using Discord.WebSocket;

namespace UKFursBot;

public interface IMessageEditedHandler
{
    Task HandleMessageUpdated(Cacheable<IMessage,ulong> before, SocketMessage after, ISocketMessageChannel channel);
}