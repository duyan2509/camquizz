using System.Text;
using CamQuizz.Application.Dtos;
using CamQuizz.Application.Interfaces;
using CamQuizz.Domain.Entities;
using CamQuizz.Domain;
using CamQuizz.Persistence.Interfaces;
namespace CamQuizz.Application.Services;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;

    public GenreService(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task<GenreDto> UpdateAsync(Guid id,UpdateGenreDto updateGenreDto)
    {
        var genre = await _genreRepository.GetByIdAsync(id);
        if(genre==null)
            throw new KeyNotFoundException();

        var existName = await _genreRepository.CheckExistNameAsync(id, updateGenreDto.Name);
        if (existName)
            throw new InvalidOperationException("Genre name already exists.");
        else
        {
            genre.Name = updateGenreDto.Name;
            var result = await _genreRepository.UpdateAsync(genre);
            return new GenreDto
            {
                Id = result.Id,
                Name = result.Name
            };
        }
    }
    public async Task<GenreDto> CreateAsync(CreateGenreDto createGenreDto)
    {
        var exists = await _genreRepository.CheckExistNameAsync(createGenreDto.Name);
        if (exists)
            throw new InvalidOperationException("Genre name already exists.");
        else
        {
            var genre = new Genre
            {
                Name = createGenreDto.Name
            };
            var result = await _genreRepository.AddAsync(genre);
            return new GenreDto
            {
                Id = result.Id,
                Name = result.Name
            };
        }
    }
    public async Task<ICollection<GenreDto>> GetAllAsync()
    {
        var result = await _genreRepository.GetAllAsync();
        return result.Select(g => new GenreDto
        {
            Id = g.Id,
            Name = g.Name
        }).ToList();
    }
    public async Task<GenreDto> GetByIdAsync(Guid guid)
    {
        var exist = await _genreRepository.GetByIdAsync(guid);
        return exist == null
            ? throw new InvalidOperationException("Genre is not found.")
            : new GenreDto
        {
            Id = exist.Id,
            Name = exist.Name
        };
    }
    public async Task<bool> DeleteAsync(Guid guid)
    {
        var exist = await _genreRepository.GetByIdAsync(guid);

        if (exist == null)
            throw new InvalidOperationException("Genre is not found.");

        await _genreRepository.SoftDeleteAsync(guid);
        return true; 
    }


}