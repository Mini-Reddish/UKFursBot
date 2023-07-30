namespace UKFursBot.Commands.CommandClassAttributes;

[AttributeUsage(AttributeTargets.Class)]
public class CommandDescriptionAttribute : Attribute
{
    public string Description { get; init; }
    public CommandDescriptionAttribute(string description)
    {
        Description = description;
    }
}