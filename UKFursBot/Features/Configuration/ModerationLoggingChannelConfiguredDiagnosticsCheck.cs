using Discord;
using Discord.WebSocket;
using UKFursBot.Context;

namespace UKFursBot.Features.Configuration;

public class ModerationLoggingChannelConfiguredDiagnosticsCheck : IDiagnosticsCheck
{
    private readonly UKFursBotDbContext _dbContext;
    private readonly DiscordSocketClient _client;

    public ModerationLoggingChannelConfiguredDiagnosticsCheck(UKFursBotDbContext dbContext,  DiscordSocketClient client)
    {
        _dbContext = dbContext;
        _client = client;
    }
    public DiagnosticsResult PerformCheck()
    {
        var settings = _dbContext.BotConfigurations.First();

        if (settings.ModerationLoggingChannel == 0)
        {
            return new DiagnosticsResult()
            {
                Status = DiagnosticsStatus.Error,
                ReasonForFailure = "Moderation Log channel is not set"
            };
        }
        
        var moderationLoggingChannel = _client.GetChannel(settings.ModerationLoggingChannel);

        if (moderationLoggingChannel == null  || moderationLoggingChannel is not ITextChannel)
        {
            return new DiagnosticsResult()
            {
                Status = DiagnosticsStatus.Error,
                ReasonForFailure = "The Moderation Log channel is set to an invalid channel"
            };
        }

        return DiagnosticsResult.SuccessfulResult;
    }

    public string Name => "Moderation Log Channel";
}