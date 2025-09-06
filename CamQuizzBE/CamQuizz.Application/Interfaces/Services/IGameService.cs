using CamQuizz.Application.Dtos;

namespace CamQuizz.Application.Interfaces.Services;

public interface IGameService
{
    Task<CreateRoomSuccess> CreateRoomAsync(string connectionId, Guid userId, Guid quizId);
    Task<JoinRoomSuccess>  JoinRoomAsync(string connectionId, Guid userId, string code);
    Task CheckAccess(Guid userId, Guid quizId);
    Task<GameRoom> GetRoomAsync(string code);
    Task<string> GetHostId(string code);

    Task CheckInUniqueRoom(string connectionId);

}


// GameService  -->  RedisQuizSessionCache
// Create Room (Check Access, Get Full Quizz, Get Host) --> Initial Session
// Join Room (Check Access, Get User) --> Add Player
// Leave Room --> Leave(Check Host, Switch Host || Delete Room)
// Start Game --> Start Game (return First Question) [ get first question ]
// Submit Answer --> Submit Answer (return Result for saving result in Service) [check answer]
// End Question --> EndQuestion(Get Top 10 Players, Current Rank) [GetRank]
// Next Question --> Next Question [next question]
// End Quizz --> End Quizz (return player ranks)
