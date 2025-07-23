using System.ComponentModel.DataAnnotations;

namespace CamQuizz.Domain.Entities;

public class GroupMessage: BaseEntity
{
    public Guid GroupId { get; set; }
    public Guid UserId { get; set; }
    [Required, StringLength(500)]
    public string Message { get; set; }
    public virtual Group Group { get; set; }
    public virtual User User { get; set; }
    public virtual List<UserGroup> LastReadMembers { get; set; } = new List<UserGroup>();
}