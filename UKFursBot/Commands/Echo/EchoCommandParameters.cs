using UKFursBot.Commands.CommandClassAttributes;

namespace UKFursBot.Commands.Echo;

public class EchoCommandParameters
{
    [CommandParameterDescription("This returns the string you pass into the command")]
    public string EchoResponseString { get; init; }
}