using Discord;
using Discord.WebSocket;

namespace UKFursBot;

public static class DiscordSocketClientExtensions
{
    public static async Task<ITextChannel> GetTextChannelAsync(this DiscordSocketClient client, ulong channelId)
    {
        return (ITextChannel) await client.GetChannelAsync(channelId);
    }
}