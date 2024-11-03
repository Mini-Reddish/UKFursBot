using System.Reflection;
using Discord;

namespace UKFursBot.SlashCommandParameterOptionStrategies;

public class LongSlashCommandParameterOptionStrategy : BaseSlashCommandParameterOptionStrategy<long>
{
    public override void AddOption(SlashCommandBuilder builder,PropertyInfo propertyInfo, string name, string? description, bool isRequired = false)
    {
        builder.AddOption(name, ApplicationCommandOptionType.Number, description, isRequired,minValue: long.MinValue, maxValue: long.MaxValue);
    }
}