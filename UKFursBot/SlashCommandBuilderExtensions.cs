using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Discord;
using Discord.Interactions.Builders;
using Discord.WebSocket;
using UKFursBot.Commands;
using UKFursBot.Commands.CommandClassAttributes;

namespace UKFursBot;

public static class SlashCommandBuilderExtensions
{
    public static Discord.SlashCommandBuilder BuildOptionsFromParameters(this Discord.SlashCommandBuilder builder, ISlashCommand command)
    {
        var slashCommandInterfaceType = command.GetType().GetInterfaces().FirstOrDefault(x => x.GetGenericTypeDefinition() == typeof(ISlashCommand<>));
        if (slashCommandInterfaceType == null)
            return builder;
        
        var commandPropertiesType = slashCommandInterfaceType.GetGenericArguments().FirstOrDefault();
        if (commandPropertiesType == null || commandPropertiesType == typeof(SocketSlashCommand))
            return builder;

        var commandProperties = commandPropertiesType.GetProperties();
        
        foreach (var propertyInfo in commandProperties)
        {
            var description = "No description here";
            var name = propertyInfo.Name.ToLowerInvariant();
            var propertyDescriptionAttribute = propertyInfo.GetCustomAttribute(typeof(CommandParameterDescriptionAttribute)) as CommandParameterDescriptionAttribute;
            var isRequired = propertyInfo.GetCustomAttribute<CommandParameterRequiredAttribute>()?.IsRequired ?? false;
            if (propertyDescriptionAttribute != null)
                description = propertyDescriptionAttribute.Description;
            
            if (propertyInfo.PropertyType == typeof(string))
            {
                builder.AddOption(name, ApplicationCommandOptionType.String, description, isRequired: isRequired);
            }

            if (propertyInfo.PropertyType == typeof(SocketTextChannel))
            {
                  builder.AddOption(name, ApplicationCommandOptionType.Channel, description,  isRequired: isRequired);
            }

            if (propertyInfo.PropertyType == typeof(SocketGuildUser))
            {
                builder.AddOption(name, ApplicationCommandOptionType.User, description, isRequired);
            }

            if (propertyInfo.PropertyType.IsEnum)
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
      
        return builder;
    }
}