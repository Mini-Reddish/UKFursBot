using Discord;
using UKFursBot.Context;

namespace UKFursBot.Features.GrantMember;

public class IsWelcomeMessageConfiguredDiagnosticsCheck : IDiagnosticsCheck
{
    private readonly UKFursBotDbContext _dbContext;
    private readonly IDiscordClient _discordClient;

    public IsWelcomeMessageConfiguredDiagnosticsCheck( UKFursBotDbContext dbContext, IDiscordClient discordClient)
    {
        _dbContext = dbContext;
        _discordClient = discordClient;
    }
    public DiagnosticsResult PerformCheck()
    {
        var config = _dbContext.BotConfigurations.First();

        if (config.MemberWelcomeChannelId == 0)
        {
            return new DiagnosticsResult
            {
                Status = DiagnosticsStatus.Error,
                ReasonForFailure = "There is no channel specified for the welcome message."
            };
        }

        if (config.MemberWelcomeMessage is null or "")
        {
            return new DiagnosticsResult
            {
                Status = DiagnosticsStatus.Warning,
                ReasonForFailure = "There is no welcome message specified"
            };
        }

        var guild = _discordClient.GetGuildAsync(config.GuildId).GetAwaiter().GetResult();
        var channel = guild.GetTextChannelAsync(config.MemberWelcomeChannelId).GetAwaiter().GetResult();

        if (channel is null)
        {
            return new DiagnosticsResult
            {
                Status = DiagnosticsStatus.Error,
                ReasonForFailure = "The welcome channel is not assigned to a valid channel in this server"
            };
        }

        return DiagnosticsResult.SuccessfulResult;
    }

    public string Name => "Is the welcome message configured correctly";
}