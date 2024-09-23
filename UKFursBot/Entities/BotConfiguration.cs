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
    public virtual ulong ModerationLoggingChannel { get; set; }
    public virtual ulong GuildId { get; set; }
    public virtual ulong ModMailChannel { get; set; }
    public virtual bool UserJoinLoggingEnabled { get; set; }
    public virtual string ModMailResponseMessage { get; set; } = String.Empty;
}