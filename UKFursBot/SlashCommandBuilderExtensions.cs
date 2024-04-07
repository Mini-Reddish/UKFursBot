using System.Reflection;
using Discord;
using Discord.WebSocket;
using UKFursBot.Commands.CommandClassAttributes;

namespace UKFursBot;

public static class SlashCommandBuilderExtensions
{
    public static Discord.SlashCommandBuilder BuildOptionsFromParameters(this Discord.SlashCommandBuilder builder, ISlashCommand command)
    {
        var commandBaseType = command.GetType().BaseType;
        if (commandBaseType == null)
            throw new Exception($"Cannot get base type.  All commands must inherit {typeof(BaseCommand<>).FullName}");
        
        var commandPropertiesType = commandBaseType.GetGenericArguments().FirstOrDefault();
        if (commandPropertiesType == null)
            return builder;

        var commandProperties = commandPropertiesType.GetProperties();
        
        foreach (var propertyInfo in commandProperties)
        {
            var description = "No description here";
            var name = propertyInfo.Name.ToLowerCaseWithUnderscores();
            var propertyDescriptionAttribute = propertyInfo.GetCustomAttribute(typeof(CommandParameterDescriptionAttribute)) as CommandParameterDescriptionAttribute;
            var isRequired = propertyInfo.GetCustomAttribute<CommandParameterRequiredAttribute>()?.IsRequired ?? false;
            if (propertyDescriptionAttribute != null)
                description = propertyDescriptionAttribute.Description;
            
            if (propertyInfo.PropertyType == typeof(string))
            {
                builder.AddOption(name, ApplicationCommandOptionType.String, description, isRequired: isRequired);
            }
            else if (propertyInfo.PropertyType == typeof(SocketTextChannel))
            {
                  builder.AddOption(name, ApplicationCommandOptionType.Channel, description,  isRequired: isRequired);
            }
            else if (propertyInfo.PropertyType == typeof(SocketGuildUser))
            {
                builder.AddOption(name, ApplicationCommandOptionType.User, description, isRequired);
            }
            else if (propertyInfo.PropertyType == typeof(bool))
            {
                builder.AddOption(name, ApplicationCommandOptionType.Boolean, description, isRequired);
            }
            else if (propertyInfo.PropertyType == typeof(SocketRole))
            {
                builder.AddOption(name, ApplicationCommandOptionType.Role, description, isRequired);
            }
            else if (propertyInfo.PropertyType == typeof(ulong))
            {
                builder.AddOption(name, ApplicationCommandOptionType.String, description, isRequired);
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