using System.Reflection;
using Discord;
using Discord.WebSocket;

namespace UKFursBot.SlashCommandParameterOptionStrategies;

public class SocketGuildUserSlashCommandParameterOptionStrategy : BaseSlashCommandParameterOptionStrategy<SocketGuildUser>
{
    public override void AddOption(SlashCommandBuilder builder,PropertyInfo propertyInfo, string name, string? description, bool isRequired = false)
    {
        builder.AddOption(name, ApplicationCommandOptionType.User, description, isRequired);
    }
}