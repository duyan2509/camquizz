using System.ComponentModel.DataAnnotations;

namespace CamQuizz.Domain.Entities;

public class User : BaseEntity
{
    [Required]
    [StringLength(100)]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [StringLength(255)]
    public string PasswordHash { get; set; } = string.Empty;
    
    [StringLength(100)]
    public string? FirstName { get; set; }
    
    [StringLength(100)]
    public string? LastName { get; set; }
    
    [StringLength(20)]
    public string? PhoneNumber { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public DateTime? LastLoginAt { get; set; }

    public Role Role {get; set;}
    public int RoleId {get;set;}
    public virtual ICollection<Quizz> Quizzes { get; set; }
} 