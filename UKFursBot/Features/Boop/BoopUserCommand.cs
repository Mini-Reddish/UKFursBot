using Discord.WebSocket;
using UKFursBot.Commands;
using UKFursBot.Context;

namespace UKFursBot.Features.Boop;
public class BoopUserCommand(UKFursBotDbContext dbContext, SocketMessageChannelManager socketMessageChannelManager)
    : BaseCommand<BoopUserCommandParameters>(socketMessageChannelManager)
{
    private static DateTime _datetimeOfLastBoop = DateTime.MinValue;

    public override string CommandName => "Boop";
    public override string CommandDescription => "Boops you.  UwU";

    protected override async Task Implementation(SocketSlashCommand socketSlashCommand, BoopUserCommandParameters commandParameters)
    {
        var cooldown = dbContext.BotConfigurations.First().MinutesThresholdBetweenBoops;
        if (cooldown == 0)
        {
            return;
        }
        
        if (DateTime.UtcNow < _datetimeOfLastBoop.AddMinutes(cooldown))
        {
            return;
        }
        await socketSlashCommand.Channel.SendMessageAsync($"*Gently boops <@{commandParameters.User.Id}> on the nose!*");
    }
}

public class BoopUserCommandParameters 
{
    [CommandParameterRequired]
    public SocketGuildUser User { get; set; }
}