using Discord;

namespace UKFursBot.Entities;

public class ModBanExemption
{
    public ulong Id { get; set; }
    public PermissionTarget RoleType { get; set; }
}