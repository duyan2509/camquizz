using CamQuizz.Application.Dtos;
using CamQuizz.Domain;
using CamQuizz.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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
        [HttpDelete("{id}")]
        public async Task<ActionResult<QuizzDto>> HardDelete(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var response = await _quizzService.DeleteQuiz(id);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPatch("{id}")]
        public async Task<ActionResult<QuizzDto>> UpdateQuizzInfo(Guid id, [FromBody] UpdateQuizzDto updateQuizzDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var response = await _quizzService.UpdateQuizInfoAsync(id, updateQuizzDto);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("my-quizzes")]
        [Authorize]
        public async Task<ActionResult<PagedResultDto<QuizzInfoDto>>> GetMyQuizzes(
            [FromQuery] PagedRequestDto request, [FromQuery] QuizzStatus? quizzStatus
        )
        {
            Guid userId = GetCurrentUserId();
            var result = await _quizzService.GetMyQuizzesAsync(request.Page, request.Size, quizzStatus, userId);
            return Ok(result);
        }
    }
}
