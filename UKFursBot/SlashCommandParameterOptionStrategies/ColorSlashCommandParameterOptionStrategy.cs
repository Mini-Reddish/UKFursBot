using System.Reflection;
using Discord;

namespace UKFursBot.SlashCommandParameterOptionStrategies;

public class ColorSlashCommandParameterOptionStrategy : BaseSlashCommandParameterOptionStrategy<Color>
{
    public override void AddOption(SlashCommandBuilder builder, PropertyInfo propertyInfo, string name, string? description,
        bool isRequired = false)
    {
        var fields = propertyInfo.PropertyType.GetFields(BindingFlags.Static | BindingFlags.Public);
        var choices = new List<ApplicationCommandOptionChoiceProperties>();
        foreach (var field in fields.OrderBy(x => x.Name))
        {
            if(field.Name == "MaxDecimalValue")
                continue;
                    
            choices.Add(new ApplicationCommandOptionChoiceProperties(){Name = field.Name, Value = field.Name});
        }
        builder.AddOption(name, ApplicationCommandOptionType.String, description, isRequired, choices: choices.ToArray());

    }
}