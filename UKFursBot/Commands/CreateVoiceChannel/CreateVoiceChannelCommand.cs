using Discord;
using Discord.WebSocket;
using UKFursBot.Commands.CommandClassAttributes;
using UKFursBot.Context;

namespace UKFursBot.Commands.CreateVoiceChannel;

[CommandName("Create_voice_channel")]
[CommandDescription("Creates a voice channel with the given name.")]
public class CreateVoiceChannelCommand : ISlashCommand<CreateVoiceChannelCommandParameters>
{
    public void MapSocketSlashCommandToParameters(SocketSlashCommand socketSlashCommand)
    {
        CommandParameters = socketSlashCommand.Data.MapDataToType<CreateVoiceChannelCommandParameters>();
    }

    public async Task Execute(UKFursBotDbContext context, SocketSlashCommand socketSlashCommand)
    {
        if (socketSlashCommand.Channel is SocketTextChannel socketTextChannel)
        {
            await socketTextChannel.Guild.CreateVoiceChannelAsync(CommandParameters.Channel_Name, properties =>
            {
                var permissions = new List<Overwrite>();

                //target id here is the role
                permissions.Add(new Overwrite(353232897928593410, PermissionTarget.Role,
                    new OverwritePermissions(viewChannel: PermValue.Allow, speak: PermValue.Allow)));

                properties.PermissionOverwrites = permissions;

            });
        }
    }

    public async Task OnSuccessfulCommandCompletion(UKFursBotDbContext context, SocketSlashCommand socketSlashCommand)
    {
        //TODO: Give the user that created a voice channel, a quick message/reminder what they need to do to invite people. 
    }

    public CreateVoiceChannelCommandParameters CommandParameters { get; set; }
}

public class CreateVoiceChannelCommandParameters
{
    [CommandParameterRequired]
    public string Channel_Name { get; set; }
}