using Discord;
using UKFursBot.Context;

namespace UKFursBot.Features.GrantMember;

public class IsRoleConfiguredDiagnosticCheck : IDiagnosticsCheck
{
    private readonly UKFursBotDbContext _dbContext;
    private readonly IDiscordClient _discordClient;

    public IsRoleConfiguredDiagnosticCheck(UKFursBotDbContext dbContext, IDiscordClient discordClient)
    {
        _dbContext = dbContext;
        _discordClient = discordClient;
    }
    public DiagnosticsResult PerformCheck()
    {
        var configuration = _dbContext.BotConfigurations.First();

        if (configuration.MemberRoleId == 0)
        {
            return new DiagnosticsResult()
            {
                Status = DiagnosticsStatus.Error,
                ReasonForFailure = $"Member role is not assigned",
            };
        }

        
        var guild = _discordClient.GetGuildAsync(configuration.GuildId).GetAwaiter().GetResult();
        var memberRole = guild.GetRoleAsync(configuration.MemberRoleId).GetAwaiter().GetResult();

        if (memberRole is null)
        {
            return new DiagnosticsResult()
            {
                Status = DiagnosticsStatus.Error,
                ReasonForFailure = "Member role is not assigned to a valid role in this server."
            };
        }
        
        return DiagnosticsResult.SuccessfulResult;
    }

    public string Name => "Is a role configured for the Grant Member command?";
}