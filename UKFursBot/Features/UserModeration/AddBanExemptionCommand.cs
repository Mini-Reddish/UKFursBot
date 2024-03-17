using Discord;
using Discord.WebSocket;
using UKFursBot.Commands.CommandClassAttributes;
using UKFursBot.Context;
using UKFursBot.Entities;

namespace UKFursBot.Features.UserModeration;

[CommandName("Add_ban_exemption")]
[CommandDescription("Adds a role/user so they can not be accidentally banned")]
public class AddBanExemptionCommand : BaseCommand<AddBanExemptionCommandParameters>
{
    private readonly UKFursBotDbContext _dbContext;

    public AddBanExemptionCommand(UKFursBotDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    protected override async Task Implementation(SocketSlashCommand socketSlashCommand, AddBanExemptionCommandParameters commandParameters)
    {
        if (commandParameters.User == null && commandParameters.Role == null)
        {
            await FollowupAsync("A user, or role must be specified");
            return;
        }

        var id = commandParameters?.Role?.Id ?? commandParameters?.User?.Id ?? 0;
        var type = commandParameters?.Role != null ? PermissionTarget.Role : PermissionTarget.User;

        if (_dbContext.BanExemptions.FirstOrDefault(x => x.Id == id) != null)
        {
            await FollowupAsync($"{type.ToString()} has already been added to the list of exemptions");
            return;
        }

        _dbContext.BanExemptions.Add(new ModBanExemption()
        {
            Id = id,
            RoleType = type
        });
        await _dbContext.SaveChangesAsync();
    }
}

public class AddBanExemptionCommandParameters 
{
    public SocketRole? Role { get; set; }
    public SocketGuildUser? User { get; set; }
}