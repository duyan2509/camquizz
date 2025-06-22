using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CamQuizz.Domain.Entities
{
    public class Question : BaseEntity
    {
        [StringLength(500, MinimumLength = 2)]
        public required string Content { get; set; }

        public string? Image { get; set; }
        public int DurationSecond { get; set; }
        public double Point { get; set; }
        public Guid QuizzId { get; set; }

        public virtual Quizz Quizz { get; set; }
        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }
}
