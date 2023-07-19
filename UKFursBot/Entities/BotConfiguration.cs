using System.ComponentModel.DataAnnotations;

namespace UKFursBot.Entities;

public class BotConfiguration
{
    [Key]
    public virtual long Id { get; set; }
    
    public virtual ulong GuildId { get; set; }
    public virtual ulong AnnouncementChannelId { get; set; }
    public virtual ulong ErrorLoggingChannelId { get; set; }
    
    public virtual ulong UserJoinLoggingChannelId { get; set; }
}