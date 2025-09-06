using AutoMapper;
using CamQuizz.Application.Dtos;
using CamQuizz.Application.Exceptions;
using CamQuizz.Application.Interfaces;
using CamQuizz.Application.Interfaces.Services;
using CamQuizz.Domain;
using NanoidDotNet;

namespace CamQuizz.Application.Services;

public class GameService:IGameService
{
    private readonly IQuizSessionCache _cache;
    private readonly IQuizzRepository _quizzRepository;
    private readonly IMapper _mapper;
    private readonly IQuizzShareRepository _quizzShareRepository;
    private readonly IUserRepository _userRepository;

    public GameService(IQuizSessionCache cache, IQuizzRepository quizzRepository, IMapper mapper, IQuizzShareRepository quizzShareRepository, IUserRepository userRepository)
    {
        _cache = cache;
        _quizzRepository = quizzRepository;
        _mapper = mapper;
        _quizzShareRepository = quizzShareRepository;
        _userRepository = userRepository;
    }

    public async Task CheckAccess(Guid userId, Guid quizId)
    {
        var isAccess = await _quizzShareRepository.CheckAccessAsync(userId, quizId);
        if(!isAccess)  throw new ForbiddenException("You do not have access to this quiz");
    }

    public async Task CheckInUniqueRoom(string connectionId)
    {
        var isInOtherRoom = await _cache.ExistInOtherRoomAsync(connectionId);
        if (isInOtherRoom)
            throw new UniqueRoomException("You are in another room.\n Can't inside more than one room");
    }
    public async Task<CreateRoomSuccess> CreateRoomAsync(string connectionId, Guid userId, Guid quizId)
    {
        await CheckInUniqueRoom(connectionId);
        var quiz = await _quizzRepository.GetFullByIdAsync(quizId);
        if (quiz == null)
            throw new NotFoundException("Quizz is not found");
        if (quiz.Status != QuizzStatus.Public && quiz.AuthorId != userId)
            await CheckAccess(userId, quizId);
        var host = await _userRepository.GetByIdAsync(userId);
        if (host == null)
            throw new NotFoundException("Host is not found");
        var hostPlayer = _mapper.Map<Player>(host);
        hostPlayer.ConnectionId = connectionId;
        var questions = quiz.Questions.Select(question=>_mapper.Map<QuestionDto>(question)).ToList();
        var players = new List<Player> { hostPlayer };
        var session = new QuizSession
        {
            QuizId = quizId,
            RoomId = Guid.NewGuid().ToString(),
            Code = await Nanoid.GenerateAsync(size: 6),
            Host = hostPlayer,
            Players = players,
            Questions = questions,
        };
        await _cache.SaveSessionAsync(session, GameStatus.Waiting);
        var quizInfo = _mapper.Map<QuizzInfoDto>(quiz);
        await _cache.SaveInfoAsync(session.RoomId, quizInfo, GameStatus.Waiting);
        await _cache.SaveCodeMappingAsync(session.Code,session.RoomId);
        await _cache.SaveConnectionAsync(connectionId, session.RoomId);
        return new CreateRoomSuccess
        {
            Code = session.Code,
            QuizInfo = quizInfo,
            HostId = userId,
            RoomId = session.RoomId,
            Players = players,
        };
    }

    public async Task<JoinRoomSuccess> JoinRoomAsync(string connectionId, Guid userId, string code)
    {
        await CheckInUniqueRoom(connectionId);
        var session = await _cache.GetSessionByCodeAsync(code);
        if (session == null)
            throw new NotFoundException($"Room for ${code} is not found");
        await CheckAccess(userId, session.QuizId);
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new NotFoundException("User is not found");
        var newPlayer = _mapper.Map<Player>(user);
        newPlayer.ConnectionId = connectionId;
        if (session.Players.All(p => p.UserId != userId))
        {
            session.Players.Add(newPlayer);
        }

        await _cache.SaveSessionAsync(session, GameStatus.Waiting);
        var quizInfo = await _cache.GetInfoQuizAsync(session.RoomId);
        if (quizInfo == null)
            throw new NotFoundException("Quiz is not found");

        return new JoinRoomSuccess
        {
            CallerMessage = new CreateRoomSuccess
            {
                Code = session.Code,
                QuizInfo = quizInfo,
                HostId = session.Host.UserId,
                RoomId = session.RoomId,
            },
            OthersInGroupMessage = newPlayer
        };
    }
    public async Task<GameRoom> GetRoomAsync(string code)
    {
        var session = await _cache.GetSessionByCodeAsync(code);
        if (session == null)
            throw new NotFoundException($"Room for ${code} is not found");
        return new GameRoom
        {
            RoomId = session.RoomId,
            Players = session.Players,
            Host = session.Host,
        };
    }

    public async Task<string> GetHostId(string code)
    {
        var session = await _cache.GetSessionByCodeAsync(code);
        if (session == null)
            throw new NotFoundException($"Room for ${code} is not found");
        return session.Host.UserId.ToString();
    }
}
