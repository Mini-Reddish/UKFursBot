using System.Reflection;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using UKFursBot.Commands;
using UKFursBot.SlashCommandParameterOptionStrategies;

namespace UKFursBot;

public static class SlashCommandBuilderExtensions
{
    public static SlashCommandBuilder BuildOptionsFromParameters(this SlashCommandBuilder builder, Type commandType, SlashCommandParameterOptionStrategyResolver slashCommandParameterOptionStrategyResolver)
    {
        var commandBaseType = commandType.BaseType;
        if (commandBaseType == null || commandBaseType.Name != typeof(BaseCommand<>).Name)
            throw new Exception($"Cannot get base type.  All commands must inherit {typeof(BaseCommand<>).FullName}");
        
        var commandPropertiesType = commandBaseType.GetGenericArguments().FirstOrDefault();
        if (commandPropertiesType == null)
            return builder;

        var commandProperties = commandPropertiesType.GetProperties();
                
        foreach (var propertyInfo in commandProperties)
        {
            ISlashCommandParameterOptionStrategy? resolver;
            if (propertyInfo.PropertyType.IsEnum)
            {
                resolver = slashCommandParameterOptionStrategyResolver.Resolve(propertyInfo.PropertyType.BaseType?.Name ?? String.Empty);
            }
            else
            {
                resolver = slashCommandParameterOptionStrategyResolver.Resolve(propertyInfo.PropertyType.Name);
            }
            if (resolver == null)
            {
                Console.WriteLine($"Unknown property type: {propertyInfo.PropertyType.FullName}");
                continue;
            }
            var description = "No description here";
            var name = propertyInfo.Name.ToLowerCaseWithUnderscores();
            var propertyDescriptionAttribute = propertyInfo.GetCustomAttribute(typeof(CommandParameterDescriptionAttribute)) as CommandParameterDescriptionAttribute;
            var isRequired = propertyInfo.GetCustomAttribute<CommandParameterRequiredAttribute>()?.IsRequired ?? false;
            if (propertyDescriptionAttribute != null)
                description = propertyDescriptionAttribute.Description;
            
            resolver.AddOption(builder, propertyInfo, name, description, isRequired);
        }
      
        return builder;
    }
}