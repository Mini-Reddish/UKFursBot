namespace UKFursBot.Commands;

public class CommandParameterDescriptionAttribute : Attribute
{
    public string Description { get; init; }
    public CommandParameterDescriptionAttribute(string description)
    {
        Description = description;
    }
}