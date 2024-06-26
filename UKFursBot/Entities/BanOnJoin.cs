﻿using System.ComponentModel.DataAnnotations;

namespace UKFursBot.Entities;

public  class BanOnJoin
{
    [Key]
    public virtual long Id { get; set; }
    public virtual ulong UserId { get; set; }
    public virtual ulong ModeratorId { get; set; }
    public virtual string Reason { get; set; } = string.Empty;
}