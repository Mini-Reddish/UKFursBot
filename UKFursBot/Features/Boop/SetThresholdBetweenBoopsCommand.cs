using System.ComponentModel.DataAnnotations;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using UKFursBot.Commands;
using UKFursBot.Context;

namespace UKFursBot.Features.Boop;
public class SetThresholdBetweenBoopsCommand : BaseCommand<SetThresholdBetweenBoopsCommandParameters>

{
    private readonly UKFursBotDbContext _dbContext;

    public SetThresholdBetweenBoopsCommand(UKFursBotDbContext dbContext)
    {
        _dbContext = dbContext;

    }

    public override string CommandName => "set_boop_threshold";

    public override string CommandDescription =>
        "Set the threshold in minutes of the boop command.  0 minutes will disable this feature.";

    protected override async Task Implementation(SocketSlashCommand socketSlashCommand, SetThresholdBetweenBoopsCommandParameters commandParameters)
    {
        var config = await _dbContext.BotConfigurations.FirstAsync();

        config.MinutesThresholdBetweenBoops = commandParameters.Minutes;
    }
    
    
}

public class SetThresholdBetweenBoopsCommandParameters   
{
    [Required]
    public long Minutes { get; set; }
}