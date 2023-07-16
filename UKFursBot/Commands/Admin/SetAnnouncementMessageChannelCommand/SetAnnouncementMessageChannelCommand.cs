using Discord.WebSocket;
using UKFursBot.Commands.CommandClassAttributes;
using UKFursBot.Context;
using UKFursBot.Entities;

namespace UKFursBot.Commands.Admin.SetAnnouncementMessageChannelCommand;

[CommandName("Set_announcement_message_channel")]
[CommandDescription("Sets the channel in which the announcement messages are sent to")]
public class SetAnnouncementMessageChannelCommand : ISlashCommand<SetAnnouncementMessageChannelCommandParameters>
{
    public void MapSocketSlashCommandToParameters(SocketSlashCommand socketSlashCommand)
    {
        var options = socketSlashCommand.Data.Options;
        CommandParameters = new SetAnnouncementMessageChannelCommandParameters()
        {
            Channel = options.FirstOrDefault(x => string.Equals(x.Name, nameof(SetAnnouncementMessageChannelCommandParameters.Channel), StringComparison.InvariantCultureIgnoreCase))?.Value as SocketTextChannel
        };
    }

    public async Task Execute(UKFursBotDbContext context)
    {
        var botConfiguration = context.BotConfigurations.FirstOrDefault();
        if (botConfiguration == null)
        {
            await context.BotConfigurations.AddAsync(new BotConfiguration()
            {
                AnnouncementChannelId = CommandParameters.Channel.Id
            });
        }
        else
        {
            botConfiguration.AnnouncementChannelId = CommandParameters.Channel.Id;
            context.BotConfigurations.Update(botConfiguration);
        }
    }

    public SetAnnouncementMessageChannelCommandParameters CommandParameters { get; set; }
}

public class SetAnnouncementMessageChannelCommandParameters     
{
    [CommandParameterRequired(true)]
    public SocketTextChannel Channel { get; set; }
}