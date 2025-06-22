using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CamQuizz.Domain.Entities
{
    public class Answer : BaseEntity
    {
        [StringLength(120, MinimumLength = 2)]
        public required string Content { get; set; }

        public string? Image { get; set; }
        public bool IsCorrect { get; set; }
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }

    }
}
