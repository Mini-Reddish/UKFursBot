using System.ComponentModel.DataAnnotations;

namespace UKFursBot.Entities;

public partial class ErrorLogging
{
    [Key]
    public long Id { get; set; }
    public ulong ServerId { get; set; }
    public ulong? ChannelId { get; set; }
    public bool IsEnabled { get; set; }
}