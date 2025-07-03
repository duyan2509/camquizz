using System.ComponentModel.DataAnnotations;

namespace CamQuizz.Application.Dtos;

public class CreateGroupDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; }
}

public class GroupDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; }
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public string OwnerName{ get; set; }
    public string AmountSharedQuizz { get; set; }
}
public class FullGroupDto
{
    public string Name { get; set; }
    public Guid Id { get; set; }
    public ICollection<MemberDto> Members { get; set; } = new List<MemberDto>();
    public ICollection<QuizzDto> Quizzes { get; set; } = new List<QuizzDto>();
    public Guid OwnerId { get; set; }
    public string OwnerName{ get; set; }
}
public class UpdateGroupDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; }

}

public class LeaveGroupDto
{
    [Required]
    public Guid GroupId { get; set; }
}
