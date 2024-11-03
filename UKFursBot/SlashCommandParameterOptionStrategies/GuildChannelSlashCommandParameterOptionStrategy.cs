using System.Reflection;
using Discord;
using SlashCommandBuilder = Discord.SlashCommandBuilder;

namespace UKFursBot.SlashCommandParameterOptionStrategies;

public class GuildChannelSlashCommandParameterOptionStrategy : BaseSlashCommandParameterOptionStrategy<IGuildChannel>
{
    public override void AddOption(SlashCommandBuilder builder,PropertyInfo propertyInfo, string name, string? description, bool isRequired = false)
    {
        builder.AddOption(name, ApplicationCommandOptionType.Channel, description, isRequired: isRequired);
    }
}