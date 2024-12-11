namespace UKFursBot.Commands;

public class CommandManager(IServiceProvider serviceProvider) : ICommandManager
{
    IDictionary<string, Type> _commands = new Dictionary<string, Type>();

    public bool AddSlashCommand(ISlashCommand slashCommand)
    {
        return _commands.TryAdd(slashCommand.CommandName.ToLowerInvariant(), slashCommand.GetType());
    }
    
    public bool RemoveSlashCommand(string commandName)
    {
        return _commands.Remove(commandName.ToLowerInvariant());
    }

    public void RemoveAllSlashCommands()
    {
        _commands.Clear();
    }

    public IEnumerable<ISlashCommand> GetAllSlashCommands()
    {
        return _commands.Values.Select(x => serviceProvider.GetService(x) as ISlashCommand)!;
    }

    public ISlashCommand? TryGetSlashCommand(string commandName)
    {
        return _commands.TryGetValue(commandName.ToLowerInvariant(), out var slashCommand) ? serviceProvider.GetService(slashCommand) as ISlashCommand : null;
    }
}

public interface ICommandManager
{
    bool AddSlashCommand(ISlashCommand slashCommand);

    bool RemoveSlashCommand(string commandName);

    void RemoveAllSlashCommands();
    IEnumerable<ISlashCommand> GetAllSlashCommands();
    ISlashCommand? TryGetSlashCommand(string commandName);
}