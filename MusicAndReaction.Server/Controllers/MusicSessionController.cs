using Microsoft.AspNetCore.Mvc;

namespace MusicAndReaction.Server.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;

    [ApiController]
    [Route("api/[controller]")]
    public class MusicSessionController : ControllerBase
    {
        private static readonly List<string> MusicTracks = new()
        {
            "/music/track1.mp3",
            "/music/track2.mp3",
            "/music/track3.mp3"
        };

        private static readonly Random RandomGen = new();

        [HttpGet]
        public IActionResult GetMusicSession()
        {
            // Losowy wybór utworu
            var selectedTrack = MusicTracks[RandomGen.Next(MusicTracks.Count)];

            // Losowy czas zatrzymania muzyki (3 - 7 sekund)
            var stopTime = RandomGen.Next(1000, 90000);

            return Ok(new
            {
                trackUrl = selectedTrack,
                stopTime = stopTime
            });
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class ReactionController : ControllerBase
    {
        public class ReactionData
        {
            public int ReactionTime { get; set; }
        }

        [HttpPost]
        public IActionResult PostReaction([FromBody] ReactionData data)
        {
            Console.WriteLine($"Otrzymano czas reakcji: {data.ReactionTime} ms");
            return Ok(new { message = "Czas reakcji zapisany" });
        }
    }

}
