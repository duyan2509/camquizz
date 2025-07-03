using CamQuizz.Application.Dtos;
using CamQuizz.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CamQuizz.Presentation.Controllers;

public class GenreController : BaseController
{
    private readonly IGenreService _genreService;

    public GenreController(IGenreService genreService)
    {
        _genreService = genreService;
    }
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<GenreDto>> CreateGenre([FromBody] CreateGenreDto createGenreDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var response = await _genreService.CreateAsync(createGenreDto);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<GenreDto>> UpdateGenre(Guid id, [FromBody] UpdateGenreDto updateGenreDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var response = await _genreService.UpdateAsync(id, updateGenreDto);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<GenreDto>> GetById(Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var response = await _genreService.GetByIdAsync(id);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    [HttpGet]
    public async Task<ActionResult<ICollection<GenreDto>>> GetAll()
    {
        try
        {
            var response = await _genreService.GetAllAsync();
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult<GenreDto>> DeleteById(Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var response = await _genreService.DeleteAsync(id);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
