using Discord.WebSocket;
using UKFursBot.Commands.CommandClassAttributes;

namespace UKFursBot.Commands;

public class EchoCommandParameters
{
    [CommandParameterDescription("This returns the string you pass into the command")]
    public string EchoResponseString { get; init; }
    public ISocketMessageChannel Channel { get; set; }
}