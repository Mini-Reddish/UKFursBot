using System.Reflection;
using Discord;
using Discord.WebSocket;
using UKFursBot.Commands.CommandClassAttributes;
using UKFursBot.Context;

namespace UKFursBot.Commands.Ban;

[CommandName("ban")]
[CommandDescription("Ban the specified user, immediately or the next time they join if they have already left.")]
public class BanCommand : ISlashCommand<BanCommandParameters>
{

    private readonly DiscordSocketClient _client;
    private readonly SocketMessageChannelManager _socketMessageChannelManager;

    public BanCommand(DiscordSocketClient client, SocketMessageChannelManager socketMessageChannelManager)
    {
        _client = client;
        _socketMessageChannelManager = socketMessageChannelManager;
    }
    public void MapSocketSlashCommandToParameters(SocketSlashCommand socketSlashCommand)
    {
        CommandParameters = socketSlashCommand.Data.MapDataToType<BanCommandParameters>();
    }

    public async Task Execute(UKFursBotDbContext context, SocketSlashCommand socketSlashCommand)
    {
        try
        {
            var dmChannel = await CommandParameters.User.CreateDMChannelAsync();
            await dmChannel.SendMessageAsync(CommandParameters.BanMessage);
        }
        catch (Exception e)
        {
            await _socketMessageChannelManager.SendLoggingErrorMessageAsync("Unable to DM User", e);
            //Log that we failed to send a DM and why if needed.
        }

        try
        {
            await CommandParameters.User.BanAsync(0, CommandParameters.BanMessage);
        }
        catch (Exception e)
        {
            await _socketMessageChannelManager.SendLoggingErrorMessageAsync("Unable to Ban User", e);
        }
        //Log action in mod log channel
    }

    public BanCommandParameters CommandParameters { get; set; }
}

public class BanCommandParameters   
{
    [CommandParameterRequired]
    public SocketGuildUser User { get; set; }
    
    
    [CommandParameterRequired]
    public string BanMessage { get; set; }
}