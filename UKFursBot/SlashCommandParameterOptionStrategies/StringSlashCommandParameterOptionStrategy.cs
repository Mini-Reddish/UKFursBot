using System.Reflection;
using Discord;
using SlashCommandBuilder = Discord.SlashCommandBuilder;

namespace UKFursBot.SlashCommandParameterOptionStrategies;

public class StringSlashCommandParameterOptionStrategy : ISlashCommandParameterOptionStrategy
{
    public Type AssociatedType => typeof(string);

    public void AddOption(SlashCommandBuilder builder, PropertyInfo propertyInfo, string name, string? description, bool isRequired = false)
    {
        builder.AddOption(name, ApplicationCommandOptionType.String, description, isRequired: isRequired);
    }
}