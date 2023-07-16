namespace UKFursBot.Commands.CommandClassAttributes;

public class CommandParameterRequiredAttribute : Attribute
{
    public bool IsRequired => true;
}