using Discord;
using Discord.WebSocket;
using UKFursBot.Commands;
using UKFursBot.Context;
using UKFursBot.Entities;

namespace UKFursBot.Features.Configuration;
public class SetChannelForCommand : BaseCommand<SetChannelForCommandParameters>
{
    private readonly UKFursBotDbContext _dbContext;

    public SetChannelForCommand(UKFursBotDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override string CommandName => "admin_set_channel_for";
    public override string CommandDescription => "Set the channel setting to output in the specified channel";

    protected override async Task Implementation(SocketSlashCommand socketSlashCommand, SetChannelForCommandParameters commandParameters)
    {
        if (commandParameters.Channel is not ITextChannel)
        {
            await FollowupAsync("The specified channel is invalid.  You must specify a text channel.");
            return;
        }
        
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
            case AdminMessageTypes.ModMail:
                botConfiguration.ModMailChannel = commandParameters.Channel.Id;
                break;
            case AdminMessageTypes.MemberWelcome:
                botConfiguration.MemberWelcomeChannelId = commandParameters.Channel.Id;
                break;
        }

        if (isCreating == false)
        {
            _dbContext.BotConfigurations.Update(botConfiguration);
        }
        
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
    public required IGuildChannel Channel { get; set; }
    
}