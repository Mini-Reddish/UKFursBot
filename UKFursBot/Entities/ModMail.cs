using System.ComponentModel.DataAnnotations;

namespace UKFursBot.Entities;

public  class ModMail
{
    [Key]
    public long Id { get; set; }
    public ulong ServerId { get; set; }
    public bool IsEnabled { get; set; }
    public ulong? ModRoleId { get; set; }
    public ulong? ChannelId { get; set; }
}