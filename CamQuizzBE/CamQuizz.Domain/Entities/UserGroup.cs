using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CamQuizz.Domain.Entities
{
    public class UserGroup : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid GroupId { get; set; }
        public Guid? LastReadMessageId { get; set; }
        public virtual GroupMessage LastReadMessage { get; set; }
        public virtual User User { get; set; }
        public virtual Group Group { get; set; } 
    }
}
