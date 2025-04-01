using MusicAndReaction.Server.Database;

public interface IDatabaseService
{
    Task AddReaction(ReactionDTO newReaction);
    Task<List<ReactionDTO>> GetAllDataList();
    Task<List<double>> GetDataForMusic(int trackid);
    Task<List<double>> GetOrderByForMusic(int trackid);
    Task<List<ReactionDTO>> GetOrderByDataList();
}