using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CamQuizz.Domain.Entities
{
    public class Quizz : BaseEntity
    {
        [StringLength(120, MinimumLength = 2)]
        public string Name { get; set; }

        public string? Image { get; set; }

        public Genre Genre { get; set; }

        public Guid? GenreId { get; set; }

        public QuizzStatus Status { get; set; } = QuizzStatus.Public;
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
        public int NumberOfAttemps { get; set; }
        public Guid AuthorId { get; set; }
        public User Author { get; set; }
        public virtual ICollection<QuizzShare> QuizzShares { get; set; } = new List<QuizzShare>();
    }
}
