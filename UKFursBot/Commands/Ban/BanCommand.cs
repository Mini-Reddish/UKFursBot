using Discord.WebSocket;
using UKFursBot.Commands.CommandClassAttributes;
using UKFursBot.Context;

namespace UKFursBot.Commands.Ban;

[CommandName("ban")]
[CommandDescription("Ban the specified user, immediately or the next time they join if they have already left.")]
public class BanCommand : ISlashCommand<BanCommandParameters>
{
    public void MapSocketSlashCommandToParameters(SocketSlashCommand socketSlashCommand)
    {
        throw new NotImplementedException();
    }

    public async Task Execute(UKFursBotDbContext context)
    {
        throw new NotImplementedException();
    }

    public BanCommandParameters CommandParameters { get; set; }
}

public class BanCommandParameters   
{
}