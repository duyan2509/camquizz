using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CamQuizz.Application.Dtos;

public class CreateMemberDto
{
    [Required]
    public Guid GroupId { get; set; }
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; }
}


public class UserGroupDto
{
    public Guid Id { get; set; }
}

public class MemberDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; }
    public bool IsOwner { get; set; } = false;
}