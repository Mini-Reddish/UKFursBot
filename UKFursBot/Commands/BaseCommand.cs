using Discord;
using Discord.Rest;
using Discord.WebSocket;

namespace UKFursBot.Commands;

public abstract class BaseCommand<T> : ISlashCommand
{
    private bool _hasFollowedUp;
    protected virtual bool SkipDefer => false;
    protected virtual bool IsEphemeral => true;
    private SocketSlashCommand? _socketSlashCommand;
    public async Task Execute(SocketSlashCommand socketSlashCommand, T commandParameters)
    {
        _socketSlashCommand = socketSlashCommand;
        if(!SkipDefer)
            await socketSlashCommand.DeferAsync(ephemeral: IsEphemeral);
        
        await Implementation(socketSlashCommand, commandParameters);
        await FollowupAsync("Task completed");
    }

    protected Task<RestFollowupMessage?> FollowupAsync(string response)
    {
        if (_hasFollowedUp == false && _socketSlashCommand != null)
        {
            _hasFollowedUp = true;
            return _socketSlashCommand.FollowupAsync(response, ephemeral: IsEphemeral);
        }

        return Task.FromResult<RestFollowupMessage?>(null);
    } 
    protected Task<RestFollowupMessage?> FollowupAsync(Embed response)
    {
        if (_hasFollowedUp == false && _socketSlashCommand != null)
        {
            _hasFollowedUp = true;
            return _socketSlashCommand.FollowupAsync(embed: response, ephemeral: IsEphemeral);
        }

        return Task.FromResult<RestFollowupMessage?>(null);
    } 
    

    protected abstract Task Implementation(SocketSlashCommand socketSlashCommand, T commandParameters);
}

public interface ISlashCommand
{

}