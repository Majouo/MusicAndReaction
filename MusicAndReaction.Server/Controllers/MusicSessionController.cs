using Microsoft.AspNetCore.Mvc;
using MusicAndReaction.Server.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicAndReaction.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MusicSessionController : ControllerBase
{

    private static readonly Random RandomGen = new();

    private readonly IDatabaseService _databaseService;

    public MusicSessionController(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMusicSession()
    {
        // Losowy wybór utworu
        int tracksCount = await _databaseService.GetMusicTrackCount();
        int random = RandomGen.Next(1,tracksCount);
        MusicFileDto selectedTrack = await _databaseService.GetMusicTrackGetById(random);

        var stopTime = RandomGen.Next(1000, 30000);

        return Ok(new
        {
            trackId = random,
            trackUrl = "/music/"+selectedTrack.FileName,
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

