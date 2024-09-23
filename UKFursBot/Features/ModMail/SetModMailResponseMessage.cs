using Discord.WebSocket;
using UKFursBot.Commands;
using UKFursBot.Context;

namespace UKFursBot.Features.ModMail;


[CommandName("set_modmail_response")]
[CommandDescription("Sets the response given to a user when they send in a modmail message")]
public class SetModMailResponseMessage  : BaseCommand<SetModMailResponseMessageCommandParameters>
{
    private readonly UKFursBotDbContext _dbContext;

    public SetModMailResponseMessage(UKFursBotDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    protected override async Task Implementation(SocketSlashCommand socketSlashCommand, SetModMailResponseMessageCommandParameters commandParameters)
    {
        var botConfig = _dbContext.BotConfigurations.First();

        if (string.IsNullOrWhiteSpace(commandParameters.Message))
        {
            await FollowupAsync("Please specify a valid message");
            return;
        }

        botConfig.ModMailResponseMessage = commandParameters.Message;
        
    }
}

public class SetModMailResponseMessageCommandParameters
{
    [CommandParameterRequired]
    public required string Message { get; set; }
}