namespace UKFursBot;

public class DiagnosticsResult
{
    public static  DiagnosticsResult SuccessfulResult = new(){Status = DiagnosticsStatus.Success};

    public DiagnosticsStatus Status { get; set; }
    public string ReasonForFailure { get; set; } = "All good!";
}

public enum DiagnosticsStatus
{
    Success,
    Warning,
    Error
}