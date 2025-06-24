using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CamQuizz.Domain.Entities
{
    public class QuizzShare : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid GroupId { get; set; }
        public Guid QuizzId { get; set; }

        public virtual User User { get; set; }
        public virtual Group Group { get; set; }
        public virtual Quizz Quizz { get; set; }
        public bool IsHide { get; set; } = false;


        // 1 User : N QuizzShare
        // N QuizzShare : 1 Group
        // N QuizzShare : 1 Quizz
        
    }
}
