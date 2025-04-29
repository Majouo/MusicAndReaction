using Microsoft.AspNetCore.Mvc;
using MusicAndReaction.Server.Database;
using System;
using System.Collections.Generic;

namespace MusicAndReaction.Server.Controllers;

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
        int random = RandomGen.Next(MusicTracks.Count);
        var selectedTrack = MusicTracks[random];

        // Losowy czas zatrzymania muzyki (3 - 7 sekund)
        var stopTime = RandomGen.Next(1000, 90000);

        return Ok(new
        {
            trackId = random,
            trackUrl = selectedTrack,
            stopTime = stopTime
        });
    }
}

[ApiController]
[Route("api/[controller]")]
public class ReactionController : ControllerBase
{

    private readonly IDatabaseService _databaseService;

    public ReactionController(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    [HttpPost]
    public async Task<IActionResult> PostReaction([FromBody] ReactionDTO data)
    {
        Console.WriteLine($"Otrzymano czas reakcji: {data.ReactionTime} ms");

        try
        {
            await _databaseService.AddReaction(data);
        }
        catch (Exception ex)
        {

            Console.WriteLine(ex.ToString());
            return StatusCode(500, "Nie udało się zapisac wyniku");
        }

        return Ok(new { message = "Czas reakcji zapisany" });
    }

    [HttpGet]
    public async Task<IActionResult> GetScores()
    {
        try
        {
            var scores = await _databaseService.GetOrderByDataList();

            return Ok(scores);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return StatusCode(500, "Nie udało się pobrać wyników");
        }
    }

    [HttpGet("{trackId}")]
    public async Task<IActionResult> GetScoresForTrack(int trackId)
    {
        try
        {
            var scores = await _databaseService.GetOrderByForMusic(trackId);

            return Ok(scores);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return StatusCode(500, "Nie udało się pobrać wyników");
        }
    }
}

