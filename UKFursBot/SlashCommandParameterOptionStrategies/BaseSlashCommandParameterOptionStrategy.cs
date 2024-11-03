using System.Reflection;
using Discord;

namespace UKFursBot.SlashCommandParameterOptionStrategies;

public abstract class BaseSlashCommandParameterOptionStrategy<T> : ISlashCommandParameterOptionStrategy
{
    public Type AssociatedType => typeof(T);

    public abstract void AddOption(SlashCommandBuilder builder, PropertyInfo propertyInfo, string name, string? description,bool isRequired = false);
}