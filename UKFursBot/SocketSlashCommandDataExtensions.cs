using System.Reflection;
using Discord.WebSocket;

namespace UKFursBot;

public static  class SocketSlashCommandDataExtensions
{
    public static T MapDataToType<T>(this SocketSlashCommandData data) where T : new()
    {
        var result = new T();
        var resultType = typeof(T);
        foreach (var parameter in data.Options)
        {
            var propertyRef = resultType.GetProperty(parameter.Name,
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

        return result;
    }
}