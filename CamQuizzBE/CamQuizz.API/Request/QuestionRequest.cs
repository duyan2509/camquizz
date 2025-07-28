using System.ComponentModel.DataAnnotations;
using CamQuizz.Application.Dtos;

namespace CamQuizz.Request;

public class CreateQuestionRequest
{
    public required string Content { get; set; }
    public IFormFile? Image { get; set; }
    public int DurationSecond { get; set; }
    public double Point { get; set; }
    [MinLength(2, ErrorMessage = "Each question must have at least 2 answers.")]
    public required ICollection<CreateAnswerDto> Answers { get; set; } = new List<CreateAnswerDto>();
}
public class UpdateQuestionRequest
{
    public required string Content { get; set; }
    public IFormFile? Image { get; set; }

    public int DurationSecond { get; set; }
    public double Point { get; set; }
    [MinLength(2, ErrorMessage = "Each question must have at least 2 answers.")]
    public required List<AnswerDto> Answers { get; set; } = new List<AnswerDto>();
}