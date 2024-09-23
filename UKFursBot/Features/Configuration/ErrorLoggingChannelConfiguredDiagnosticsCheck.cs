using Discord;
using Discord.WebSocket;
using UKFursBot.Context;

namespace UKFursBot.Features.Configuration;

public class ErrorLoggingChannelConfiguredDiagnosticsCheck : IDiagnosticsCheck
{
    private readonly UKFursBotDbContext _dbContext;
    private readonly DiscordSocketClient _client;

    public ErrorLoggingChannelConfiguredDiagnosticsCheck(UKFursBotDbContext dbContext,  DiscordSocketClient client)
    {
        _dbContext = dbContext;
        _client = client;
    }
    public DiagnosticsResult PerformCheck()
    {
        var settings = _dbContext.BotConfigurations.First();

        if (settings.ErrorLoggingChannelId == 0)
        {
            return new DiagnosticsResult()
            {
                Status = DiagnosticsStatus.Error,
                ReasonForFailure = "ErrorLogging channel is not set"
            };
        }
        
        var errorLoggingChannel = _client.GetChannel(settings.ErrorLoggingChannelId);

        if (errorLoggingChannel == null  || errorLoggingChannel is not ITextChannel)
        {
            return new DiagnosticsResult()
            {
                Status = DiagnosticsStatus.Error,
                ReasonForFailure = "The ErrorLogging channel is set to an invalid channel"
            };
        }

        return DiagnosticsResult.SuccessfulResult;
    }

    public string Name => "ErrorLogging Channel";
}