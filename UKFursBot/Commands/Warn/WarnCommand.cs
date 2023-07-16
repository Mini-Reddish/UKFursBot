using Discord.WebSocket;
using UKFursBot.Commands.CommandClassAttributes;
using UKFursBot.Context;

namespace UKFursBot.Commands.Warn;

[CommandName("warn")]
[CommandDescription("Send a warning to the current user")]
public class WarnCommand : ISlashCommand<WarnCommandParameters>
{
    public void MapSocketSlashCommandToParameters(SocketSlashCommand socketSlashCommand)
    {
        throw new NotImplementedException();
    }

    public async Task Execute(UKFursBotDbContext context,  SocketSlashCommand socketSlashCommand)
    {
        throw new NotImplementedException();
    }

    public WarnCommandParameters CommandParameters { get; set; }
}

public class WarnCommandParameters      
{
}