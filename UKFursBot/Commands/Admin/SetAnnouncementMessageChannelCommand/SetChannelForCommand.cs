using Discord;
using Discord.WebSocket;
using UKFursBot.Commands.CommandClassAttributes;
using UKFursBot.Context;
using UKFursBot.Entities;

namespace UKFursBot.Commands.Admin.SetAnnouncementMessageChannelCommand;

[CommandName("admin_set_channel_for")]
[CommandDescription("Set the channel setting to output in the specified channel")]
public class SetChannelForCommand : ISlashCommand<SetChannelForCommandParameters>
{
    public void MapSocketSlashCommandToParameters(SocketSlashCommand socketSlashCommand)
    {
        CommandParameters = socketSlashCommand.Data.MapDataToType<SetChannelForCommandParameters>();
    }
    public async Task Execute(UKFursBotDbContext context, SocketSlashCommand socketSlashCommand)
    {
        var botConfiguration = context.BotConfigurations.FirstOrDefault();
        bool isCreating = false;
        if (botConfiguration == null)
        {
            var result = await context.BotConfigurations.AddAsync(new BotConfiguration());
            botConfiguration = result.Entity;
            isCreating = true;
        }

        switch (CommandParameters.MessageType)
        {
            case AdminMessageTypes.Announcements:
                botConfiguration.AnnouncementChannelId = CommandParameters.Channel.Id;
                break;
            case AdminMessageTypes.ErrorLogging:
                botConfiguration.ErrorLoggingChannelId = CommandParameters.Channel.Id;
                break;
        }

        if (isCreating == false)
        {
            context.BotConfigurations.Update(botConfiguration);
        }
        
    }
    
    public SetChannelForCommandParameters CommandParameters { get; set; }
}

public class SetChannelForCommandParameters     
{
    [CommandParameterRequired]
    public AdminMessageTypes MessageType { get; set; }
    
    [CommandParameterRequired]
    public SocketTextChannel Channel { get; set; }
    
}