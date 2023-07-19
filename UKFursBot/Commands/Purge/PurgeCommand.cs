using Discord.WebSocket;
using UKFursBot.Commands.CommandClassAttributes;
using UKFursBot.Context;

namespace UKFursBot.Commands.Purge;

[CommandName("purge")]
[CommandDescription("Removes all messages posted by a user.")]
public class PurgeCommand : ISlashCommand<PurgeCommandParameters>
{
    public void MapSocketSlashCommandToParameters(SocketSlashCommand socketSlashCommand)
    {
        throw new NotImplementedException();
    }

    public async Task Execute(UKFursBotDbContext context,  SocketSlashCommand socketSlashCommand)
    {
        throw new NotImplementedException();
    }

    public async Task OnSuccessfulCommandCompletion(UKFursBotDbContext context, SocketSlashCommand socketSlashCommand)
    {
        //TODO:  Log that users messages were purged.
    }

    public PurgeCommandParameters CommandParameters { get; set; }
}

public class PurgeCommandParameters
{
}