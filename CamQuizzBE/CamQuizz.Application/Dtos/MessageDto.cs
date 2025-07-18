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

public class ConversationDto
{
    public string GroupName { get; set; }
    public Guid GroupId { get; set; }
    public MessageDto? LastMessage { get; set; }
    public int UnreadCounts { get; set; }
}
public class ConversationPreview
{
    public Guid GroupId { get; set; }
    public string GroupName { get; set; }
    public string LastMessage { get; set; }
    public string SenderName { get; set; }
    public DateTime? LastMessageAt { get; set; }
    public int UnreadCount { get; set; }
}