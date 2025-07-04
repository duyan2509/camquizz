using CamQuizz.Domain;
using System.ComponentModel.DataAnnotations;

namespace CamQuizz.Application.Dtos
{
    public class CreateQuizzDto
    {
        [StringLength(120, MinimumLength = 2)]

        public string Name { get; set; }

        public string? Image { get; set; }

        public Guid GenreId { get; set; }
        public Guid AuthorId { get; set; }
        public QuizzStatus Status { get; set; } = QuizzStatus.Public;
        public ICollection<CreateQuestionDto> Questions { get; set; } = new List<CreateQuestionDto>();
        public List<Guid> GroupIds { get; set; } = new List<Guid>();
        public int NumberOfAttemps { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
    public class QuizzDto
    {
        public Guid Id { get; set; }
        [StringLength(120, MinimumLength = 2)]

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
    public class UpdateQuizzDto
    {
        [StringLength(120, MinimumLength = 2)]
        public string? Name { get; set; }

        public string? Image { get; set; }

        public Guid? GenreId { get; set; }
        public QuizzStatus? Status { get; set; }

    }
    public class QuizzInfoDto
    {
        public Guid Id { get; set; }
        [StringLength(120, MinimumLength = 2)]

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
    public class GroupQuizzInfoDto
    {
        public Guid QuizzId { get; set; }
        [StringLength(120, MinimumLength = 2)]
        public Guid Id { get; set; }
        [StringLength(120, MinimumLength = 2)]

        public string Name { get; set; }

        public string? Image { get; set; }

        public Guid GenreId { get; set; }
        public Guid AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string GenreName { get; set; }
        public QuizzStatus Status { get; set; } = QuizzStatus.Public;
        public int NumberOfQuestions { get; set; }
        public int NumberOfAttemps { get; set; } = 0;
        public DateTime ShareAt { get; set; } = DateTime.UtcNow;
        public bool isHide { get; set; }
    }
    public class UpdateAccessDto
    {
        public QuizzStatus Status { get; set; }
        public List<Guid> GroupIds { get; set; } = new List<Guid>();
    }
    public class QuizzAccessDto
    {
        public Guid Id { get; set; }
        [StringLength(120, MinimumLength = 2)]

        public string Name { get; set; }

        public string? Image { get; set; }
        public QuizzStatus Status { get; set; }
        public List<AccessDto> Accesses { get; set; } = new List<AccessDto>();
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class UpdateQuizzVisibleDto
    {
        public bool Visible {get; set;} 
    }
}
