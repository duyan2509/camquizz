using CamQuizz.Application.Dtos;
using CamQuizz.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CamQuizz.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizzController : BaseController
    {
        private readonly IQuizzService _quizzService;

        public QuizzController(IQuizzService quizzService)
        {
            _quizzService = quizzService;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<QuizzDto>> GetQuizzById(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var response = await _quizzService.GetQuizInfoByIdAsync(id);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost]
        public async Task<ActionResult<QuizzDto>> CreateQuizz([FromBody] CreateQuizzDto createQuizzDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var response = await _quizzService.CreateAsync(createQuizzDto);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
