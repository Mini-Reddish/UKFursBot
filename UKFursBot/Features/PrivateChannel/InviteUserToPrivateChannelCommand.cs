using Discord;
using Discord.Net;
using Discord.WebSocket;
using UKFursBot.Commands;

namespace UKFursBot.Features.PrivateChannel;
public class InviteUserToPrivateChannelCommand : BaseCommand<InviteUserToPrivateChannelCommandParameters>
{
    private readonly SocketMessageChannelManager _messageChannelManager;

    public InviteUserToPrivateChannelCommand( SocketMessageChannelManager messageChannelManager)
    {
        _messageChannelManager = messageChannelManager;
    }

    public override string CommandName => "Invite_to_private";
    public override string CommandDescription => "Invite a user to your current private channel.";

    protected override async Task Implementation(SocketSlashCommand socketSlashCommand, InviteUserToPrivateChannelCommandParameters commandParameters)
    {
        if(socketSlashCommand.User is not SocketGuildUser userOfCommand)
            return;

        var usersPrivateRole = userOfCommand.Roles.FirstOrDefault(x => x.Name.StartsWith(PrivateChannelConstants.PrivateChannelPrefix));
        if (usersPrivateRole == null)
        {
            await FollowupAsync($"Cannot invite the user <@{commandParameters.User.Id}> as you have yet to create a private chat.");
            return;
        }

        var targetsPrivateRole = commandParameters.User.Roles.FirstOrDefault(x => x.Name.StartsWith(PrivateChannelConstants.PrivateChannelPrefix));
        if (targetsPrivateRole != null)
        {
            await FollowupAsync($"Cannot invite the user <@{commandParameters.User.Id}> as they are already in a private chat");
            return;
        }

        await commandParameters.User.AddRoleAsync(usersPrivateRole);

        

        var privateChannel = userOfCommand.Guild.Channels.First(x => x.Name == usersPrivateRole.Name);
        if (privateChannel == null)
        {
            await _messageChannelManager.SendLoggingWarningMessageAsync("A user tried to invite someone to a private channel however the channel does not exist.  This suggests that the private channel system is set up invalid.");
            await FollowupAsync($"Cannot invite the user <@{commandParameters.User.Id}> as the channel does not exist.");
            return;
        }
          
        var richTextMessage = new RichTextBuilder()
            .AddHeading2("Invitation to Private Chat")
            .AddText($"<@{userOfCommand.Id}> has invited you to join their private call <#{privateChannel.Id}>")
            .Build();
        var embed = new EmbedBuilder()
        {
            Color = Color.Gold,
            Description = richTextMessage
        }.Build();
        try
        {
            await commandParameters.User.SendMessageAsync(embed: embed);
        }
        catch (HttpException httpException)
        {
            await _messageChannelManager.SendLoggingErrorMessageAsync("Inviting user to private channel failed due to an exception.", httpException);
            await FollowupAsync($"The user <@{commandParameters.User.Id}> could not be contacted to invite them.  Please message them manually");
        }
        
        await FollowupAsync($"The user <@{commandParameters.User.Id}> has invited to the private chat!");

    }
}

public class InviteUserToPrivateChannelCommandParameters    
{
    [CommandParameterRequired]
    public required SocketGuildUser User { get; set; }
}