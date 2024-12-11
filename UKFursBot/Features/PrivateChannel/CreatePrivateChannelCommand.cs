using Discord;
using Discord.WebSocket;
using UKFursBot.Commands;

namespace UKFursBot.Features.PrivateChannel;
public class CreatePrivateChannelCommand : BaseCommand<NoCommandParameters>
{
    private readonly BotGuildUsers _botGuildUsers;

    public CreatePrivateChannelCommand(BotGuildUsers botGuildUsers)
    {
        _botGuildUsers = botGuildUsers;
    }
    
    private string GetLatestPrivateRoleName(SocketTextChannel socketTextChannel)
    {
        var existingRoles = socketTextChannel.Guild.Roles.Where(x => x.Name.StartsWith(PrivateChannelConstants.PrivateChannelPrefix)).ToList();
        var maxPrivateChannelNumber = 0;
        if (existingRoles.Count > 0)
        {
            maxPrivateChannelNumber = existingRoles.Max(x => Convert.ToInt32(x.Name.Split("-")[1]));
        }
        return PrivateChannelConstants.PrivateChannelPrefix +(maxPrivateChannelNumber + 1);
    }

    public override string CommandName => "Create_private_channel";
    public override string CommandDescription => "Creates a private voice channel.";

    protected override async Task Implementation(SocketSlashCommand socketSlashCommand, NoCommandParameters commandParameters)
    {
        if (socketSlashCommand.User is not SocketGuildUser user)
            return;

        var botUser = _botGuildUsers[user.Guild.Id];
        var botRole = botUser.Roles.First(x => x.IsManaged);
        
        if (socketSlashCommand.Channel is SocketTextChannel socketTextChannel)
        {
            var privateRole = await socketTextChannel.Guild.CreateRoleAsync(GetLatestPrivateRoleName(socketTextChannel));
            var privateCategoryId = socketTextChannel.Guild.CategoryChannels.FirstOrDefault(x => x.Name == PrivateChannelConstants.PrivateChannelCategory)?.Id ?? null;
            
            if (privateCategoryId == null)
            {
                var categoryChannel = await socketTextChannel.Guild.CreateCategoryChannelAsync(PrivateChannelConstants.PrivateChannelCategory);
                privateCategoryId = categoryChannel.Id;
            }
                
            var voiceChannel = await socketTextChannel.Guild.CreateVoiceChannelAsync(privateRole.Name, properties =>
            {
                var permissions = new List<Overwrite>();

                //target id here is the roleId
                permissions.Add(new Overwrite(privateRole.Id, PermissionTarget.Role, new OverwritePermissions(viewChannel: PermValue.Allow, speak: PermValue.Allow, connect:PermValue.Allow)));
                permissions.Add(new Overwrite(user.Guild.EveryoneRole.Id, PermissionTarget.Role, new OverwritePermissions(viewChannel: PermValue.Deny, connect: PermValue.Deny)));
                permissions.Add(new Overwrite(botRole.Id, PermissionTarget.Role, new OverwritePermissions(viewChannel:PermValue.Allow, manageChannel: PermValue.Allow, connect: PermValue.Allow )));
                properties.CategoryId = privateCategoryId;
                properties.PermissionOverwrites = permissions;
            },
            new RequestOptions());

            await user.AddRoleAsync(privateRole);
            await FollowupAsync($"I have created the private channel <#{voiceChannel.Id}> for you.  Feel free to invite others using /invite_user_to_private");
        }
        else
        {
            await FollowupAsync($"I have failed to create the private channel sorry.");
        }
    }
}