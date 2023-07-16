using System.ComponentModel.DataAnnotations;

namespace UKFursBot.Entities;

public partial class BanOnJoin
{
    [Key]
    public virtual long Id { get; set; }
    public virtual ulong UserID { get; set; }
    public virtual ulong ModID { get; set; }
    public virtual string Reason { get; set; }
}