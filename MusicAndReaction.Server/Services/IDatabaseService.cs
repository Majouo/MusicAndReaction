using MusicAndReaction.Server.Database;

public interface IDatabaseService
{
    Task AddReaction(ReactionDTO newReaction);
    Task<List<ReactionDTO>> GetAllDataList();
    Task<List<int>> GetDataForMusic(int trackid);
    Task<List<int>> GetOrderByForMusic(int trackid);
    Task<List<ReactionDTO>> GetOrderByDataList();
    Task<MusicFileDto> GetMusicTrackGetById(int trackid);
    Task<int> GetMusicTrackCount();
}