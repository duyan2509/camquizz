namespace CamQuizz.Application.Dtos;

public class MessageDto
{
    public string Message { get; set; }
    public DateTime Time { get; set; }
    public string Sender { get; set; }
    public Guid Id { get; set; }
    public Guid SenderId { get; set; }
}

public class CreateMessageDto
{
    public string Content { get; set; } = string.Empty;
}

public class CreateMessageResultDto
{
    public MessageDto? Message { get; set; }
    public List<Guid> ReceiverIds { get; set; } = new List<Guid>();
}