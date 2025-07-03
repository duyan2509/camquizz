using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CamQuizz.Domain.Entities
{
    public class Genre : BaseEntity
    {
        [Required]
        [StringLength(500, MinimumLength = 2)]
        public string Name { get; set; }
        public virtual ICollection<Quizz> Quizzes { get; set; } = new List<Quizz>();
    }
}
