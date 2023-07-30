using System.ComponentModel.DataAnnotations;

namespace UKFursBot.Entities;

public class BanLog
{
    [Key]
    public virtual long Id { get; set; }
    public virtual  DateTime DateAdded { get; set; }
    public virtual ulong UserId { get; set; }
    public virtual ulong ModeratorId { get; set; }
    public virtual string Reason { get; set; } = string.Empty;
    public bool WasSentToUser { get; set; }
}