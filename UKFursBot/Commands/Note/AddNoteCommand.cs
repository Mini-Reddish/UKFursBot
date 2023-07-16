using Discord.WebSocket;
using UKFursBot.Commands.CommandClassAttributes;
using UKFursBot.Context;

namespace UKFursBot.Commands.Note;

[CommandName("add_note")]
[CommandDescription("Add a note to the specified user")]
public class AddNoteCommand : ISlashCommand<AddNoteCommandParameters>
{
    public void MapSocketSlashCommandToParameters(SocketSlashCommand socketSlashCommand)
    {
        throw new NotImplementedException();
    }

    public async Task Execute(UKFursBotDbContext context)
    {
        throw new NotImplementedException();
    }

    public AddNoteCommandParameters CommandParameters { get; set; }
}

public class AddNoteCommandParameters       
{
}