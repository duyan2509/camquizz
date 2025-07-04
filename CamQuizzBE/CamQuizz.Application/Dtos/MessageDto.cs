namespace CamQuizz.Application.Dtos;

public class MessageDto
{
    public string Message { get; set; }
    public DateTime Time { get; set; }
    public string Sender { get; set; }
    public Guid Id { get; set; }
}

public class CreateMessageDto
{
    public string Content { get; set; } = string.Empty;
}