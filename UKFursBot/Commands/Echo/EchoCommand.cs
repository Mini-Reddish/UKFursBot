using Discord.WebSocket;
using UKFursBot.Commands.CommandClassAttributes;

namespace UKFursBot.Commands.Echo;

[CommandName("echo")]
[CommandDescription("Returns the entered string")]
public class EchoCommand : BaseCommand<EchoCommandParameters>
{
    protected override async Task Implementation(SocketSlashCommand socketSlashCommand, EchoCommandParameters commandParameters)
    {
         await socketSlashCommand.Channel.SendMessageAsync(commandParameters.EchoResponseString);
    }
}