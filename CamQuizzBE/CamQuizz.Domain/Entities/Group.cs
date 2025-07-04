using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CamQuizz.Domain.Entities
{
    public class Group : BaseEntity
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }
        public virtual ICollection<UserGroup> UserGroups { get; set; } = new List<UserGroup>();
        [Required]
        public Guid OwnerId { get; set; }
        public virtual User Owner { get; set; }
        public virtual ICollection<QuizzShare> QuizzShares { get; set; } = new List<QuizzShare>();
        public virtual ICollection<GroupMessage> GroupMessages { get; set; } = new List<GroupMessage>();
    }
}
