﻿using System.ComponentModel.DataAnnotations;

namespace UKFursBot.Entities;

public class AnnouncementMessage
{
    [Key]
    public virtual long MessageId { get; set; }
    public virtual string MessageContent { get; set; } = String.Empty;
    public virtual string MessagePurpose { get; set; } = String.Empty;
}