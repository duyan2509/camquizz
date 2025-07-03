using System.ComponentModel.DataAnnotations;
using CamQuizz.Domain;

namespace CamQuizz.Domain.Entities;

public class Role {
    public int Id {get;set;}
    public UserRole Name = UserRole.User;
    public ICollection<User> Users {get;set;} = new List<User>();
}