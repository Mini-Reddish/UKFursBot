namespace UKFursBot.Commands.CommandClassAttributes;

public class CommandNameAttribute : Attribute
{
    public string Name { get; init; }
    public CommandNameAttribute(string name)
    {
        Name = name;
    }
}