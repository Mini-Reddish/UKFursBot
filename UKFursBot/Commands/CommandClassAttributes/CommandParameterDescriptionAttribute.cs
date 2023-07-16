namespace UKFursBot.Commands.CommandClassAttributes;

public class CommandParameterDescriptionAttribute : Attribute
{
    public string Description { get; init; }
    public CommandParameterDescriptionAttribute(string description)
    {
        Description = description;
    }
}

public class CommandParameterRequiredAttribute : Attribute
{
    public bool IsRequired { get; init; }
    public CommandParameterRequiredAttribute(bool isRequired)
    {
        IsRequired = isRequired;
    }
}