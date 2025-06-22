using CamQuizz.Domain;
using CamQuizz.Domain.Entities;

namespace CamQuizz.Application.Dtos
{
    public class CreateQuizzDto
    {
        public string Name { get; set; }

        public string? Image { get; set; }

        public Guid GenreId { get; set; }
        public Guid AuthorId { get; set; }
        public QuizzStatus Status { get; set; } = QuizzStatus.Public;
        public ICollection<CreateQuestionDto> Questions { get; set; } = new List<CreateQuestionDto>();
        public int NumberOfAttemps { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
    public class QuizzDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string? Image { get; set; }

        public Guid GenreId { get; set; }
        public Guid AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string GenreName { get; set; }

        public QuizzStatus Status { get; set; } = QuizzStatus.Public;
        public ICollection<QuestionDto> Questions { get; set; } = new List<QuestionDto>();
        public int NumberOfAttemps { get; set; } = 0;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; } 
    }
    public class QuizzInfoDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string? Image { get; set; }

        public Guid GenreId { get; set; }
        public Guid AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string GenreName { get; set; }
        public QuizzStatus Status { get; set; } = QuizzStatus.Public;
        public int NumberOfQuestions { get; set; } 
        public int NumberOfAttemps { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
