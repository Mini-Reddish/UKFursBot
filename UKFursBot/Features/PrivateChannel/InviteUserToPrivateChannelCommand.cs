using Discord;
using Discord.Net;
using Discord.WebSocket;
using UKFursBot.Commands.CommandClassAttributes;

namespace UKFursBot.Features.PrivateChannel;

[CommandName("Invite_to_private")]
[CommandDescription("Invite a user to your current private channel.")]
public class InviteUserToPrivateChannelCommand : BaseCommand<InviteUserToPrivateChannelCommandParameters>
{
    protected override async Task Implementation(SocketSlashCommand socketSlashCommand, InviteUserToPrivateChannelCommandParameters commandParameters)
    {
        await socketSlashCommand.DeferAsync();
        
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
            //TODO: Log that the private channel could not be found.
            return;
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
            //TODO:  Check and prompt that a DM could not be sent to invite them so the user needs to do it manually.
        }
        
        await FollowupAsync($"The user <@{commandParameters.User.Id}> has invited to the private chat!");

    }
}

public class InviteUserToPrivateChannelCommandParameters    
{
    [CommandParameterRequired]
    public SocketGuildUser User { get; set; }
}