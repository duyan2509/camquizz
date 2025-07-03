namespace CamQuizz.Application.Dtos;

// QuizzShare : userId, quizzId, groupId 
public class AccessDto
{
    public string GroupName { get; set; }
    public Guid GroupId { get; set; }
    public DateTime ShareAt { get; set; }
}