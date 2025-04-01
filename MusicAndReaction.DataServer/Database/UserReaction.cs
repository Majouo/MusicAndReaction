using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAndReaction.DataServer.Database;
public class UserReaction
{
    public int Id { get; init; } = default;
    public required string TrackId { get; set; }
    public required TimeSpan ReactionTime { get; set; }
    public required DateTime AttemptDate { get; set; }
}
