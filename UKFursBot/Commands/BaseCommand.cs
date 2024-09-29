using Discord;
using Discord.WebSocket;

namespace UKFursBot.Commands;

public abstract class BaseCommand<T> : ISlashCommand
{
    private bool _hasFollowedUp;
    private SocketSlashCommand? _socketSlashCommand;
    public async Task Execute(SocketSlashCommand socketSlashCommand, T commandParameters)
    {
        _socketSlashCommand = socketSlashCommand;
        await socketSlashCommand.DeferAsync(ephemeral: true);
        await Implementation(socketSlashCommand, commandParameters);
        await FollowupAsync("Task completed");
    }

    protected Task FollowupAsync(string response)
    {
        if (_hasFollowedUp == false && _socketSlashCommand != null)
        {
            _hasFollowedUp = true;
            return _socketSlashCommand.FollowupAsync(response, ephemeral: true);
        }

        return Task.CompletedTask;
    } 
    protected Task FollowupAsync(Embed response)
    {
        if (_hasFollowedUp == false && _socketSlashCommand != null)
        {
            _hasFollowedUp = true;
            return _socketSlashCommand.FollowupAsync(embed: response, ephemeral: true);
        }

        return Task.CompletedTask;
    } 
    

    protected abstract Task Implementation(SocketSlashCommand socketSlashCommand, T commandParameters);
}

public interface ISlashCommand
{

}