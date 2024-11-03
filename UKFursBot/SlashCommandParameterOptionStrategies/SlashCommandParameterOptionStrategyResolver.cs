namespace UKFursBot.SlashCommandParameterOptionStrategies;

public class SlashCommandParameterOptionStrategyResolver
{
    public SlashCommandParameterOptionStrategyResolver(IEnumerable<ISlashCommandParameterOptionStrategy> strategies)
    {
        foreach (var strategy in strategies)
        {
            Register(strategy);
        }
    }
    private readonly IDictionary<string, ISlashCommandParameterOptionStrategy> _slashCommandParameterOptionStrategies = new Dictionary<string, ISlashCommandParameterOptionStrategy>();
    public ISlashCommandParameterOptionStrategy? Resolve(string name)
    {
        _slashCommandParameterOptionStrategies.TryGetValue(name, out var slashCommandParameterOptionStrategy);
        return slashCommandParameterOptionStrategy;
    }

    public void Register(ISlashCommandParameterOptionStrategy parameterOptionStrategy)
    {
        _slashCommandParameterOptionStrategies.Add(parameterOptionStrategy.AssociatedType.Name, parameterOptionStrategy);
    }
}