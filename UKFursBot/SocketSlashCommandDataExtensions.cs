using System.Reflection;
using Discord.WebSocket;

namespace UKFursBot;

public static  class SocketSlashCommandDataExtensions
{
    public static object MapDataToType(this SocketSlashCommandData data, Type type)
    {
        var result = Activator.CreateInstance(type);
        foreach (var parameter in data.Options)
        {
            var propertyRef = type.GetProperty(parameter.Name,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyRef == null)
            {
                //TODO: log error.
                continue;
            }
            
            if (propertyRef.PropertyType.IsEnum)
            {
                propertyRef.SetValue(result, Enum.Parse(propertyRef.PropertyType, parameter.Value.ToString() ?? string.Empty));
            }
            else
            {
                propertyRef.SetValue(result, parameter.Value);
            }

        }

        if (result == null)
            throw new UnableToMapException(
                $"Error trying to map the socket command data to the following type: {type.Name}");

        return result;
    }
}

public class UnableToMapException : Exception
{
    public UnableToMapException(string s) : base(s)
    {
    }
}