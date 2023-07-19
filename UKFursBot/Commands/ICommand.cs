using Discord.WebSocket;
using UKFursBot.Context;

namespace UKFursBot.Commands;

public interface ISlashCommand
{
    void MapSocketSlashCommandToParameters(SocketSlashCommand socketSlashCommand);
    Task Execute(UKFursBotDbContext context, SocketSlashCommand socketSlashCommand);
    Task OnSuccessfulCommandCompletion(UKFursBotDbContext context, SocketSlashCommand socketSlashCommand);
}

public interface ISlashCommand<T> : ISlashCommand
{
    T CommandParameters { get; set; }

   
}
