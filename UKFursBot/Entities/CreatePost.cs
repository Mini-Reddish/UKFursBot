using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Discord;
using UKFursBot.Features.Posts;

namespace UKFursBot.Entities;

public class CreatePost
{
    [Key]
    public virtual long Id { get; set; }
    
    public virtual ulong MessageId { get; set; }
    
    public virtual ulong ChannelId { get; set; }
    public virtual ulong CreatePostResponseToken { get; set; }
    public virtual CreatePostState State { get; set; } 
    public virtual DateTime ActivelyListenForResponseUntil { get; set; }
    public virtual bool UseEmbed { get; set; }
    public virtual uint EmbedColour { get; set; }
}