using CamQuizz.Domain.Entities;

namespace CamQuizz.Application.Dtos
{
    public class CreateQuestionDto
    {
        public required string Content { get; set; }

        public string? Image { get; set; }
        public int DurationSecond { get; set; }
        public double Point { get; set; }

        public required ICollection<CreateAnswerDto> Answers { get; set; } = new List<CreateAnswerDto>();
    }
    public class QuestionDto
    {
        public Guid Id { get; set; }
        public required string Content { get; set; }

        public string? Image { get; set; }
        public int DurationSecond { get; set; }
        public double Point { get; set; }

        public required List<AnswerDto> Answers { get; set; } = new List<AnswerDto>();
    }
}
