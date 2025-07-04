﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MusicAndReaction.Server.Database;
public class UserReaction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required int TrackId { get; set; }
    public int ReactionTime { get; set; }
    public string Mode { get; set; }
    public DateTime AttemptDate { get; set; }
}


