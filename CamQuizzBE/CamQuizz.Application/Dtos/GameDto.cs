namespace CamQuizz.Application.Dtos;

public class CreateRoomSuccess
{
    public required string Code{ get; set; }
    public QuizzInfoDto? QuizInfo { get; set; }
    public required Guid HostId { get; set; }
    public required string RoomId { get; set; }
    public List<Player> Players { get; set; } = new List<Player>();

}

public class JoinRoomSuccess
{
    public CreateRoomSuccess CallerMessage { get; set; } 
    public Player OthersInGroupMessage { get; set; }
}

public class MessageNotification
{
    public required string Message { get; set; }
}
public class FailMessage: MessageNotification
{
}

public class GameRoom
{
    public string RoomId { get; set; }
    public List<Player> Players { get; set; } = new List<Player>();
    public required Player Host { get; set; }
}