using Discord;
using Discord.WebSocket;
using UKFursBot.Context;

namespace UKFursBot.Features.Configuration;

public class AnnouncementsChannelConfiguredDiagnosticsCheck : IDiagnosticsCheck
{
    private readonly UKFursBotDbContext _dbContext;
    private readonly DiscordSocketClient _client;

    public AnnouncementsChannelConfiguredDiagnosticsCheck(UKFursBotDbContext dbContext, DiscordSocketClient client)
    {
        _dbContext = dbContext;
        _client = client;
    }
    public DiagnosticsResult PerformCheck()
    {
        var settings = _dbContext.BotConfigurations.First();

        if (settings.AnnouncementChannelId == 0)
        {
            return new DiagnosticsResult()
            {
                Status = DiagnosticsStatus.Error,
                ReasonForFailure = "Announcement channel is not set"
            };
        }

        var announcementChannel = _client.GetChannel(settings.AnnouncementChannelId);

        if (announcementChannel == null  || announcementChannel is not ITextChannel)
        {
            return new DiagnosticsResult()
            {
                Status = DiagnosticsStatus.Error,
                ReasonForFailure = "The Announcement channel is set to an invalid channel"
            };
        }

        return DiagnosticsResult.SuccessfulResult;
    }

    public string Name => "Announcement Channel";
}