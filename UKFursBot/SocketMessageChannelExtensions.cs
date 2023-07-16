using Discord;
using Discord.WebSocket;

namespace UKFursBot;

public static class SocketMessageChannelExtensions
{
    public static async Task SendLoggingErrorMessageAsync(this ISocketMessageChannel channel, string message,
        Exception exception)
    {
        var embedBuilder = new EmbedBuilder()
        {
            Description = $"{message}, The exception was {exception.Message} ",
            Color = Color.Red
        };

        await channel.SendMessageAsync(string.Empty, false, embedBuilder.Build());
    }
}