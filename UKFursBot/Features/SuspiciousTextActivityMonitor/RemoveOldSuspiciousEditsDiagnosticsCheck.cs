using Discord.WebSocket;
using UKFursBot.Context;

namespace UKFursBot.Features.SuspiciousTextActivityMonitor;

public class RemoveOldSuspiciousEditsDiagnosticsCheck : IDiagnosticsCheck
{
    private readonly UKFursBotDbContext _dbContext;

    public RemoveOldSuspiciousEditsDiagnosticsCheck(UKFursBotDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public DiagnosticsResult PerformCheck()
    {
        var settings = _dbContext.BotConfigurations.First();

        if (settings.MinutesThresholdForMessagesBeforeEditsAreSuspicious <= 0)
        {
            return new DiagnosticsResult()
            {
                Status = DiagnosticsStatus.Warning,
                ReasonForFailure =
                    "Removing of suspicious edits is disabled due to Minutes threshold being less than or equal to 0"
            };
        }

        return DiagnosticsResult.SuccessfulResult;
    }

    public string Name => "Remove suspicious edits checker";
}