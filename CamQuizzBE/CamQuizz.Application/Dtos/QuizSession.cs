using NanoidDotNet;

namespace CamQuizz.Application.Dtos;

public class QuizSession
{
    public Guid QuizId { get; set; }
    public required string RoomId { get; set; } = Guid.NewGuid().ToString();
    public required string Code { get; set; } = Nanoid.Generate(size: 6);
    public required Player Host { get; set; }
    public required List<Player> Players { get; set; }  = new List<Player>();
    public required List<QuestionDto> Questions { get; set; }
    public int CurrentQuestionIndex { get; set; } = 0;
}

public class Player
{
    public Guid UserId { get; set; }
    public required string Name { get; set; }
    public string ConnectionId  { get; set; }
    public double Score { get; set; } = 0;
}

public enum GameStatus
{
    Waiting,
    Running,
}