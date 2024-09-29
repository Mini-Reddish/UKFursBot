using Discord;
using Discord.WebSocket;
using UKFursBot.Context;

namespace UKFursBot.Features.UserGreeting;

public class UserJoinLoggingChannelConfiguredDiagnosticsCheck : IDiagnosticsCheck
{
    private readonly UKFursBotDbContext _dbContext;
    private readonly DiscordSocketClient _client;

    public UserJoinLoggingChannelConfiguredDiagnosticsCheck(UKFursBotDbContext dbContext,  DiscordSocketClient client)
    {
        _dbContext = dbContext;
        _client = client;
    }
    public DiagnosticsResult PerformCheck()
    {
        var settings = _dbContext.BotConfigurations.First();
        
        if (settings.UserJoinLoggingEnabled == false)
        {
            return new DiagnosticsResult()
            {
                Status = DiagnosticsStatus.Warning,
                ReasonForFailure = "UserJoin Logging is Disabled"
            };
        }
        
        if (settings.UserJoinLoggingChannelId == 0)
        {
            return new DiagnosticsResult()
            {
                Status = DiagnosticsStatus.Error,
                ReasonForFailure = "UserJoin Log channel is not set"
            };
        }
        
        var userJoinLogChannel = _client.GetChannel(settings.UserJoinLoggingChannelId);

        if (userJoinLogChannel == null  || userJoinLogChannel is not ITextChannel)
        {
            return new DiagnosticsResult()
            {
                Status = DiagnosticsStatus.Error,
                ReasonForFailure = "The User Join Log channel is set to an invalid channel"
            };
        }

        return DiagnosticsResult.SuccessfulResult;
    }

    public string Name => "User Join Channel";
}