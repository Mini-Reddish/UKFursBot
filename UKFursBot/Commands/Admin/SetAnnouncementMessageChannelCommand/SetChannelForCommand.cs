using Discord;
using Discord.WebSocket;
using UKFursBot.Commands.CommandClassAttributes;
using UKFursBot.Context;
using UKFursBot.Entities;

namespace UKFursBot.Commands.Admin.SetAnnouncementMessageChannelCommand;

[CommandName("admin_set_channel_for")]
[CommandDescription("Set the channel setting to output in the specified channel")]
public class SetChannelForCommand : BaseCommand<SetChannelForCommandParameters>
{
    private readonly UKFursBotDbContext _dbContext;

    public SetChannelForCommand(UKFursBotDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    protected override async Task Implementation(SocketSlashCommand socketSlashCommand, SetChannelForCommandParameters commandParameters)
    {
        var botConfiguration = _dbContext.BotConfigurations.FirstOrDefault();
        bool isCreating = false;
        if (botConfiguration == null)
        {
            var result = await _dbContext.BotConfigurations.AddAsync(new BotConfiguration());
            botConfiguration = result.Entity;
            isCreating = true;
        }

        switch (commandParameters.MessageType)
        {
            case AdminMessageTypes.Announcements:
                botConfiguration.AnnouncementChannelId = commandParameters.Channel.Id;
                break;
            case AdminMessageTypes.ErrorLogging:
                botConfiguration.ErrorLoggingChannelId = commandParameters.Channel.Id;
                break;
            case AdminMessageTypes.UserJoinLog:
                botConfiguration.UserJoinLoggingChannelId = commandParameters.Channel.Id;
                break;
            case AdminMessageTypes.ModerationLog:
                botConfiguration.ModerationLoggingChannel = commandParameters.Channel.Id;
                break;
        }

        if (isCreating == false)
        {
            _dbContext.BotConfigurations.Update(botConfiguration);
        }

        await _dbContext.SaveChangesAsync();
        
        var messageContents = new RichTextBuilder()
            .AddHeading1("Config Changed")
            .AddText($"I have set the {commandParameters.MessageType} messages to be sent to <#{commandParameters.Channel.Id}>")
            .AddText($"Action by <@{socketSlashCommand.User.Id}>");
        
        var embed = new EmbedBuilder()
        {
            Color = Color.Blue,
            Description = messageContents.Build()
        };
        await socketSlashCommand.Channel.SendMessageAsync(embed: embed.Build());
    }
}

public class SetChannelForCommandParameters     
{
    [CommandParameterRequired]
    public AdminMessageTypes MessageType { get; set; }
    
    [CommandParameterRequired]
    public SocketTextChannel Channel { get; set; }
    
}