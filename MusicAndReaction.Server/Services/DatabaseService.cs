﻿using Microsoft.EntityFrameworkCore;
using MusicAndReaction.Server.Database;

public class DatabaseService(AppDbContext context) : IDatabaseService
{
    #region GetData
    public async Task<List<ReactionDTO>> GetAllDataList() => await context.UserReactions.ToDto().ToListAsync();
    public async Task<List<int>> GetDataForMusic(int trackid) => await context.UserReactions.Where(x => x.TrackId == trackid).Select(x=>x.ReactionTime).ToListAsync();
    public async Task<List<int>> GetOrderByForMusic(int trackid) => await context.UserReactions.Where(x => x.TrackId == trackid).OrderBy(x => x.ReactionTime).Select(x => x.ReactionTime).ToListAsync();
    public async Task<List<ReactionDTO>> GetOrderByDataList() => await context.UserReactions.OrderBy(x => x.ReactionTime).ToDto().ToListAsync();
    public async Task<MusicFileDto> GetMusicTrackGetById(int trackid) => await context.MusicFiles.AsNoTracking().Select(m => new MusicFileDto
    {
        Id = m.Id,
        FileName = m.FileName,
        Hash = m.Hash
    }).Where(x => x.Id == trackid).FirstOrDefaultAsync();

    public async Task<int> GetMusicTrackCount() => await context.MusicFiles.CountAsync()+1;
    #endregion

    public async Task AddReaction(ReactionDTO newReaction)
    {
        await context.UserReactions.AddAsync(new UserReaction
        {
            AttemptDate = DateTime.Now,
            ReactionTime = newReaction.ReactionTime,
            TrackId = newReaction.TrackId,
            Mode = newReaction.Mode,
        });

        await context.SaveChangesAsync();
    }

}

internal static class Extension
{
    internal static IQueryable<ReactionDTO> ToDto(this IQueryable<UserReaction> query)
    {
        return query.Select(r => new ReactionDTO
        {
            TrackId = r.TrackId,
            ReactionTime = r.ReactionTime,
        });
    }
}