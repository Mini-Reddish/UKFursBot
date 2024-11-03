using System.Reflection;
using Discord;

namespace UKFursBot.SlashCommandParameterOptionStrategies;

public class BoolSlashCommandParameterOptionStrategy : BaseSlashCommandParameterOptionStrategy<bool>
{
    public override void AddOption(SlashCommandBuilder builder,PropertyInfo propertyInfo, string name, string? description, bool isRequired = false)
    {
        builder.AddOption(name, ApplicationCommandOptionType.Boolean, description, isRequired);
    }
}