using Discord.WebSocket;

namespace UKFursBot;

public interface IUserVoiceChannelChangedHandler
{
    Task HandleUserVoiceChannelChanged(SocketUser user, SocketVoiceState previousVoiceState, SocketVoiceState currentVoiceState);
}