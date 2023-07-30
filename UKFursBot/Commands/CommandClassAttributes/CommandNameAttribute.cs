namespace UKFursBot.Commands.CommandClassAttributes;

[AttributeUsage(AttributeTargets.Class)]
public class CommandNameAttribute : Attribute
{
    
    public string Name { get; init; }
    public CommandNameAttribute(string name)
    {
        Name = name;
    }
    
}