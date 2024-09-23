namespace UKFursBot;

public interface IDiagnosticsCheck
{
    DiagnosticsResult PerformCheck();
    string Name { get; }
}