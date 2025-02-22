﻿using Discord;
using Discord.WebSocket;
using UKFursBot.Commands;
using UKFursBot.Context;
using UKFursBot.Entities;

namespace UKFursBot.Features.UserModeration;
public class WarnCommand(UKFursBotDbContext dbContext, SocketMessageChannelManager socketMessageChannelManager)
    : BaseCommand<WarnCommandParameters>(socketMessageChannelManager)
{
    private readonly SocketMessageChannelManager _socketMessageChannelManager = socketMessageChannelManager;

    public override string CommandName => "warn";
    public override string CommandDescription => "Send a warning to the current user";

    protected override async Task Implementation(SocketSlashCommand socketSlashCommand, WarnCommandParameters commandParameters)
    {
        var sentToUser = false;
        try
        {
            var dmChannel = await commandParameters.User.CreateDMChannelAsync();
            await dmChannel.SendMessageAsync(commandParameters.Message);
            sentToUser = true;
        }
        catch (Exception e)
        {
            await _socketMessageChannelManager.SendLoggingErrorMessageAsync("Unable to DM User with warning", e);
        }

        dbContext.Warnings.Add(new Warning()
        {
            DateAdded = DateTime.UtcNow,
            Reason = commandParameters.Message,
            UserId = commandParameters.User.Id,
            ModeratorId = socketSlashCommand.User.Id,
            WasSentToUser = sentToUser
        });
        
        var content = new RichTextBuilder()
            .AddHeading2("Warned User")
            .AddText($"{commandParameters.User.Username} has been warned")
            .AddHeading3("Message sent to user")
            .AddText(commandParameters.Message)
            .AddHeading3("User ID")
            .AddText(commandParameters.User.Id.ToString())
            .AddHeading3("Moderator")
            .AddText($"<@{socketSlashCommand.User.Id}> | {socketSlashCommand.User.Username}")
            .AddHeading3("Was User available for DM's")
            .AddText(sentToUser ? "Yes" : "No").Build();

        var embed = new EmbedBuilder()
        {
            Color = Color.Orange,
            Description = content
        }.Build();

        await _socketMessageChannelManager.SendModerationLoggingMessageAsync(embed);
    }
}

public class WarnCommandParameters      
{
    [CommandParameterRequired]
    public required SocketGuildUser User { get; set; }
    [CommandParameterRequired]
    public required string Message { get; set; }
}