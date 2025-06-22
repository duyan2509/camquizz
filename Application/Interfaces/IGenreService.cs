using CamQuizz.Application.Dtos;

namespace CamQuizz.Application.Interfaces;

public interface IGenreService
{
    Task<GenreDto> UpdateAsync(Guid id, UpdateGenreDto updateGenreDto);
    Task<GenreDto> CreateAsync(CreateGenreDto createGenreDto);
    Task<ICollection<GenreDto>> GetAllAsync();
    Task<GenreDto> GetByIdAsync(Guid guid);
    Task<bool> DeleteAsync(Guid guid);
}