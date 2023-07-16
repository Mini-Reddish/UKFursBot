using Discord.WebSocket;
using UKFursBot.Commands.CommandClassAttributes;
using UKFursBot.Context;

namespace UKFursBot.Commands.Echo;

[CommandName("echo")]
[CommandDescription("Returns the entered string")]
public class EchoCommand : ISlashCommand<EchoCommandParameters>
{
    public EchoCommandParameters CommandParameters { get; set; }
    public void MapSocketSlashCommandToParameters(SocketSlashCommand socketSlashCommand)
    {
        var options = socketSlashCommand.Data.Options;
        CommandParameters = new EchoCommandParameters()
        {
            Channel = socketSlashCommand.Channel,
            EchoResponseString = options.FirstOrDefault(x => string.Equals(x.Name, nameof(EchoCommandParameters.EchoResponseString), StringComparison.InvariantCultureIgnoreCase))?.Value.ToString() ?? string.Empty
        };
    }

    public async Task Execute(UKFursBotDbContext context)
    {
        await CommandParameters.Channel.SendMessageAsync(CommandParameters.EchoResponseString);
    }
}