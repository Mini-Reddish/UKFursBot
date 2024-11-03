using System.Reflection;
using Discord;

namespace UKFursBot.SlashCommandParameterOptionStrategies;

public class UlongSlashCommandParameterOptionStrategy : BaseSlashCommandParameterOptionStrategy<ulong>
{
    public override void AddOption(SlashCommandBuilder builder,PropertyInfo propertyInfo, string name, string? description, bool isRequired = false)
    {
        builder.AddOption(name, ApplicationCommandOptionType.Number, description, isRequired, minValue:0, maxValue: ulong.MaxValue);
    }
}