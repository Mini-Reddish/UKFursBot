using Discord;
using Discord.WebSocket;
using UKFursBot.Context;

namespace UKFursBot.Features.Configuration;

public class ModMailChannelConfiguredDiagnosticsCheck : IDiagnosticsCheck
{
    private readonly UKFursBotDbContext _dbContext;
    private readonly DiscordSocketClient _client;

    public ModMailChannelConfiguredDiagnosticsCheck(UKFursBotDbContext dbContext,  DiscordSocketClient client)
    {
        _dbContext = dbContext;
        _client = client;
    }
    public DiagnosticsResult PerformCheck()
    {
        var settings = _dbContext.BotConfigurations.First();

        if (settings.ModMailChannel == 0)
        {
            return new DiagnosticsResult()
            {
                Status = DiagnosticsStatus.Error,
                ReasonForFailure = "ModMail channel is not set"
            };
        }
        
        var modMailChannel = _client.GetChannel(settings.AnnouncementChannelId);

        if (modMailChannel == null  || modMailChannel is not ITextChannel)
        {
            return new DiagnosticsResult()
            {
                Status = DiagnosticsStatus.Error,
                ReasonForFailure = "The ModMail channel is set to an invalid channel"
            };
        }

        if (string.IsNullOrWhiteSpace(settings.ModMailResponseMessage))
        {
            return new DiagnosticsResult()
            {
                Status = DiagnosticsStatus.Warning,
                ReasonForFailure = "ModMail does not have a response defined so no message will be sent to the user."
            };
        }

        return DiagnosticsResult.SuccessfulResult;
    }

    public string Name => "ModMail Channel";
}