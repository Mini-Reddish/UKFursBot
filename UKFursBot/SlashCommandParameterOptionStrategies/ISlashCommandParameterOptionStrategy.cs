using System.Reflection;
using SlashCommandBuilder = Discord.SlashCommandBuilder;

namespace UKFursBot.SlashCommandParameterOptionStrategies;

public interface ISlashCommandParameterOptionStrategy
{
    Type AssociatedType { get; }
    void AddOption(SlashCommandBuilder builder, PropertyInfo propertyInfo, string name, string? description, bool isRequired = false);
}