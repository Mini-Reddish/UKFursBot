using System.Reflection;
using Discord;

namespace UKFursBot.SlashCommandParameterOptionStrategies;

public class EnumSlashCommandParameterOptionStrategy : BaseSlashCommandParameterOptionStrategy<Enum>
{
    public override void AddOption(SlashCommandBuilder builder,PropertyInfo propertyInfo, string name, string? description, bool isRequired = false)
    {
        var choices = new List<ApplicationCommandOptionChoiceProperties>();
        var type = propertyInfo.PropertyType;
        var enumValues = type.GetEnumValues();
        foreach (var enumValue in enumValues)
        {
            choices.Add(new ApplicationCommandOptionChoiceProperties(){Name = enumValue.ToString(), Value = enumValue.GetHashCode().ToString()});
        }
        builder.AddOption(name, ApplicationCommandOptionType.String, description, isRequired, choices: choices.ToArray());
    }
}