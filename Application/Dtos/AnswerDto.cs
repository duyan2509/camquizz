namespace CamQuizz.Application.Dtos
{
    public class CreateAnswerDto
    {
        public required string Content { get; set; }

        public string? Image { get; set; }
        public bool IsCorrect { get; set; } = false;
    }
    public class AnswerDto
    {
        public Guid Id { get; set; }
        public required string Content { get; set; }

        public string? Image { get; set; }
        public bool IsCorrect { get; set; } 
    }
}
