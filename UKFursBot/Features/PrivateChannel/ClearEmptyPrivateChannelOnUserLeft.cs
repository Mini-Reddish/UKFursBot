using Discord.WebSocket;

namespace UKFursBot.Features.PrivateChannel;

public class ClearEmptyPrivateChannelOnUserLeft : IUserVoiceChannelChangedHandler
{
    public async Task HandleUserVoiceChannelChanged(SocketUser user, SocketVoiceState previousVoiceState,
        SocketVoiceState currentVoiceState)
    {
        var channel = previousVoiceState.VoiceChannel;
        var associatedRole = (user as SocketGuildUser)?.Roles.FirstOrDefault(x =>
                x.Name.StartsWith(PrivateChannelConstants.PrivateChannelPrefix));
        if(channel == null)
            return;

        if (channel.Name.StartsWith(PrivateChannelConstants.PrivateChannelPrefix) == false)
            return;

        if (channel.ConnectedUsers.Count > 0)
            return;

        await channel.DeleteAsync();
        if (associatedRole != null)
        {
            await associatedRole.DeleteAsync();
        }
    }
}