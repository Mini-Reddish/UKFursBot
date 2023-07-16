using System.ComponentModel.DataAnnotations;
namespace UKFursBot.Entities;
public class Warning
{
    [Key] 
    public virtual long Id { get; set; }
    public virtual DateTime DateAdded { get; set; }
    public virtual bool Forgiven { get; set; }
    public virtual ulong ForgivenBy { get; set; }
    public virtual ulong GuildId { get; set; }
    public virtual ulong ModeratorId { get; set; }
    public virtual string Reason { get; set; }
    public virtual ulong UserId { get; set; }
}
