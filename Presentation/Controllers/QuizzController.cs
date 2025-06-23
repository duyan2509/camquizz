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
        private readonly IQuestionService _questionService;


        public QuizzController(IQuizzService quizzService, IQuestionService questionService)
        {
            _quizzService = quizzService;
            _questionService = questionService;
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
                Guid userId = GetCurrentUserId();
                var response = await _quizzService.UpdateQuizInfoAsync(id, updateQuizzDto, userId);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
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
        [HttpPost("{quizzId}/question")]
        public async Task<ActionResult<QuizzDto>> AddNewQuestion(Guid quizzId, [FromBody] CreateQuestionDto createQuizzDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                Guid userId = GetCurrentUserId();
                var response = await _questionService.CreateAsync(createQuizzDto, quizzId, userId);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpDelete("{quizzId}/question/{questionId}")]
        public async Task<ActionResult<QuizzDto>> DeleteQuestion(Guid quizzId, Guid questionId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                Guid userId = GetCurrentUserId();
                var response = await _questionService.DeleteAsync(quizzId, questionId, userId);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPut("{quizzId}/question/{questionId}")]
        public async Task<ActionResult<QuizzDto>> AddNewQuestion(Guid quizzId,Guid questionId, QuestionDto questionDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                Guid userId = GetCurrentUserId();
                var response = await _questionService.UpdateAsync(quizzId, questionDto, userId);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
