using UKFursBot.Context;

namespace UKFursBot.Features.Boop;

public class BoopEnabledDiagnosticsCheck : IDiagnosticsCheck
{
    private readonly UKFursBotDbContext _dbContext;

    public BoopEnabledDiagnosticsCheck(UKFursBotDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public DiagnosticsResult PerformCheck()
    {
        if (_dbContext.BotConfigurations.First().MinutesThresholdBetweenBoops != 0)
        {
            return DiagnosticsResult.SuccessfulResult;
        }

        return new DiagnosticsResult()
        {
            ReasonForFailure = "Threshold for booping is set to 0 minutes so it is disabled.",
            Status = DiagnosticsStatus.Warning
        };
    }

    public string Name => "Boop Enabled";
}