using Discord;
using Discord.WebSocket;
using UKFursBot.Commands;

namespace UKFursBot.Features.Diagnostics;
public class SetupStatusChecklistCommand : BaseCommand<NoCommandParameters>
{
    private readonly IEnumerable<IDiagnosticsCheck> _diagnosticsChecks;

    public SetupStatusChecklistCommand(IEnumerable<IDiagnosticsCheck> diagnosticsChecks)
    {
        _diagnosticsChecks = diagnosticsChecks;
    }

    public override string CommandName => "diagnostics";
    public override string CommandDescription => "Run diagnostics checks to see if anything is set up incorrectly.";

    protected override async Task Implementation(SocketSlashCommand socketSlashCommand, NoCommandParameters commandParameters)
    {
        var response = new RichTextBuilder()
            .AddHeading2("Diagnostics Checks");
        
        foreach (var diagnosticsCheck in _diagnosticsChecks)
        {
            var result = diagnosticsCheck.PerformCheck();

            switch (result.Status)
            {
                case DiagnosticsStatus.Error:
                    response.AddText($":x: - {diagnosticsCheck.Name} - {result.ReasonForFailure}");
                    break;
                case DiagnosticsStatus.Warning:
                    response.AddText($":warning: - {diagnosticsCheck.Name} - {result.ReasonForFailure}");
                    break;
                case DiagnosticsStatus.Success:
                    response.AddText($":white_check_mark: - {diagnosticsCheck.Name}");
                    break;
            }
        }
        
        var embed = new EmbedBuilder()
        {
            Color = Color.Red,
            Description = response.Build()
        }.Build();

        await FollowupAsync(embed);
    }
}