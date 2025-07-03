using System.ComponentModel.DataAnnotations;

namespace CamQuizz.Application.Dtos;

public class CreateGenreDto
{
    [Required]
    [StringLength(20, MinimumLength = 2)]
    public string Name { get; set; }
}

public class GenreDto
{
    [Required]
    [StringLength(20, MinimumLength = 2)]
    public string Name{ get; set; }
    public Guid Id{ get; set; }
}
public class UpdateGenreDto
{
    [Required]
    [StringLength(20, MinimumLength = 2)]
    public string Name { get; set; }

}