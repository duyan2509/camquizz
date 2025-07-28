using System.ComponentModel.DataAnnotations;
using CamQuizz.Application.Dtos;
using CamQuizz.Domain;

namespace CamQuizz.Request;

public class CreateQuizzRequest
{
    [StringLength(120, MinimumLength = 2)]

    public string Name { get; set; }
    public IFormFile? Image { get; set; }

    public Guid GenreId { get; set; }
    public QuizzStatus Status { get; set; } 
    public ICollection<CreateQuestionRequest>? Questions { get; set; } = new List<CreateQuestionRequest>();
    public List<Guid>? GroupIds { get;    set; } = new List<Guid>();
}

public class UpdateInfoRequest
{
    [StringLength(120, MinimumLength = 2)]

    public string? Name { get; set; }
    public IFormFile? Image { get; set; }

    public Guid? GenreId { get; set; }
}