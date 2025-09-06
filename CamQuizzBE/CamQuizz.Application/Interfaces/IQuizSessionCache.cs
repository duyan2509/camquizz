using CamQuizz.Application.Dtos;

namespace CamQuizz.Application.Interfaces;

public interface IQuizSessionCache
{
    public Task<QuizSession?> GetSessionAsync(string roomId);

    public Task SaveSessionAsync(QuizSession session, GameStatus gameStatus);

    public Task DeleteSessionAsync(string roomId, string code);
    public Task<QuizSession?> GetSessionByCodeAsync(string code);
    public Task SaveCodeMappingAsync(string code, string roomId);
    Task SaveInfoAsync(string roomId, QuizzInfoDto info, GameStatus gameStatus);
    Task<QuizzInfoDto?> GetInfoQuizAsync(string roomId);
    public Task<bool> ExistInOtherRoomAsync(string connectionId);
    Task DeleteConnectionAsync(string connectionId);
    public Task SaveConnectionAsync(string connectionId, string roomId);
}