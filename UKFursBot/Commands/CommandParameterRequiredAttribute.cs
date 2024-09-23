namespace UKFursBot.Commands;

public class CommandParameterRequiredAttribute : Attribute
{
    public bool IsRequired => true;
}