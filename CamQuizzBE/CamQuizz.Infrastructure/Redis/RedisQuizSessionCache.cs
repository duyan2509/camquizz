using CamQuizz.Application.Dtos;
using CamQuizz.Application.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;
using IDatabase = Microsoft.EntityFrameworkCore.Storage.IDatabase;

namespace CamQuizz.Infrastructure.Redis;

public class RedisQuizSessionCache: IQuizSessionCache
{
    private readonly StackExchange.Redis.IDatabase _redis;
    private readonly string _prefix = "room:";
    private readonly string _mappingPrefix = "room-code:";
    private readonly string _infoPrefix = "room-info:";
    
    
    public RedisQuizSessionCache(IConnectionMultiplexer redis)
    {
        _redis = redis.GetDatabase();
    }

    public async Task<QuizSession?> GetSessionAsync(string roomId)
    {
        var json = await _redis.StringGetAsync($"{_prefix}{roomId}");
        return json.HasValue ? JsonConvert.DeserializeObject<QuizSession>(json!) : null;
    }
    public async Task<QuizzInfoDto?> GetInfoQuizAsync(string roomId)
    {
        var json = await _redis.StringGetAsync($"{_infoPrefix}{roomId}");
        return json.HasValue ? JsonConvert.DeserializeObject<QuizzInfoDto>(json!) : null;
    }

    public async Task<bool> ExistInOtherRoomAsync(string connectionId)
    {
        var json = await _redis.StringGetAsync(connectionId);
        var currentRoomId = json.HasValue ? JsonConvert.DeserializeObject<string>(json!) : null;
        return currentRoomId != null;
    }

    public async Task DeleteConnectionAsync(string connectionId)
    {
        await _redis.KeyDeleteAsync($"{connectionId}");
    }

    public async Task SaveConnectionAsync(string connectionId, string roomId)
    {
        await _redis.StringSetAsync($"{connectionId}", roomId, null);
    }
    
    public async Task SaveSessionAsync(QuizSession session, GameStatus gameStatus)
    {
        var json = JsonConvert.SerializeObject(session);
        await _redis.StringSetAsync($"{_prefix}{session.RoomId}", json,  gameStatus==GameStatus.Running?TimeSpan.FromMinutes(30):null);
    }

    public async Task DeleteSessionAsync(string roomId, string code)
    {
        await _redis.KeyDeleteAsync($"{_mappingPrefix}{code}");
        await _redis.KeyDeleteAsync($"{_prefix}{roomId}");
    }
    public async Task<QuizSession?> GetSessionByCodeAsync(string code)
    {
        var roomId = await _redis.StringGetAsync($"{_mappingPrefix}:{code}");
        if (!roomId.HasValue) return null;

        return await GetSessionAsync(roomId!);
    }

    public async Task SaveInfoAsync(string roomId, QuizzInfoDto info,GameStatus gameStatus)
    {
        var json = JsonConvert.SerializeObject(info);
        await _redis.StringSetAsync($"{_infoPrefix}{roomId}", json,  gameStatus==GameStatus.Running?TimeSpan.FromMinutes(30):null);
    }

    public async Task SaveCodeMappingAsync(string code, string roomId)
    {
        await _redis.StringSetAsync($"{_mappingPrefix}{code}", roomId, null  );
    }
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



