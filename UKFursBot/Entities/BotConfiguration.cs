using System.ComponentModel.DataAnnotations;

namespace UKFursBot.Entities;

public class BotConfiguration
{
    [Key]
    public virtual long Id { get; set; }
    public virtual ulong AnnouncementChannelId { get; set; }
    public virtual ulong ErrorLoggingChannelId { get; set; }
    
    public virtual ulong UserJoinLoggingChannelId { get; set; }
    public virtual long MinutesThresholdForMessagesBeforeEditsAreSuspicious { get; set; }
    public ulong ModerationLoggingChannel { get; set; }
    public ulong GuildId { get; set; }
    public ulong ModMailChannel { get; set; }
}