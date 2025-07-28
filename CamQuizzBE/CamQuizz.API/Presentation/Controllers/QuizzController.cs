using AutoMapper;
using CamQuizz.Application.Dtos;
using CamQuizz.Application.Exceptions;
using CamQuizz.Domain;
using CamQuizz.Application.Interfaces;
using CamQuizz.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CamQuizz.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuizzController : BaseController
    {
        private readonly IQuizzService _quizzService;
        private readonly IQuestionService _questionService;
        private readonly IGroupService _groupService;
        protected readonly IMapper _mapper;

        public QuizzController(IQuizzService quizzService
            , IQuestionService questionService
            , IGroupService groupService
            , IMapper mapper)
        {
            _quizzService = quizzService;
            _questionService = questionService;
            _groupService = groupService;
            _mapper = mapper;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<DetailQuizDto>> GetQuizzById(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var response = await _quizzService.GetDetailByIdAsync(id);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost]
        public async Task<ActionResult<QuizzDto>> CreateQuizz([FromForm] CreateQuizzRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var userId = GetCurrentUserId();
           
                var dto = _mapper.Map<CreateQuizzDto>(request);
                if (request.Image != null)
                {
                    var memStream = new MemoryStream();
                    await request.Image.CopyToAsync(memStream);
                    memStream.Position = 0;
                    dto.ImageStream = memStream;
                }
                var response = await _quizzService.CreateAsync(userId, dto);
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
                var userId = GetCurrentUserId();
                var response = await _quizzService.DeleteQuiz(userId, id);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ForbiddenException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPatch("{id}")]
        public async Task<ActionResult<QuizzDto>> UpdateQuizzInfo(Guid id, [FromForm] UpdateInfoRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                Guid userId = GetCurrentUserId();
                var dto = _mapper.Map<UpdateQuizzDto>(request);
                if (request.Image != null)
                {
                    var memStream = new MemoryStream();
                    await request.Image.CopyToAsync(memStream);
                    memStream.Position = 0;
                    dto.ImageStream = memStream;
                }
                var response = await _quizzService.UpdateQuizInfoAsync(id, dto, userId);
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
        public async Task<ActionResult<PagedResultDto<QuizzInfoDto>>> GetMyQuizzes(
            [FromQuery] QuizzPagedRequestDto request, [FromQuery] QuizzStatus? QuizzStatus
        )
        {
            Guid userId = GetCurrentUserId();
            var result = await _quizzService.GetMyQuizzesAsync(request.Keyword, request.CategoryId, request.Popular, request.Newest, request.Page, request.Size, QuizzStatus, userId);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PagedResultDto<QuizzInfoDto>>> GetAllQuizzes(
            [FromQuery] QuizzPagedRequestDto request
        )
        {
            var result = await _quizzService.GetAllQuizzesAsync(request.Keyword, request.CategoryId, request.Popular, request.Newest, request.Page, request.Size);
            return Ok(result);
        }
        [HttpPost("{quizzId}/question")]
        public async Task<ActionResult<QuestionDto>> AddNewQuestion(Guid quizzId, [FromForm] CreateQuestionRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                Guid userId = GetCurrentUserId();
                var dto = _mapper.Map<CreateQuestionDto>(request);
                if (request.Image != null)
                {
                    var memStream = new MemoryStream();
                    await request.Image.CopyToAsync(memStream);
                    memStream.Position = 0;
                    dto.ImageStream = memStream;
                }
                var response = await _questionService.CreateAsync(dto, quizzId, userId);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpDelete("{quizzId}/question/{questionId}")]
        public async Task<ActionResult<bool>> DeleteQuestion(Guid quizzId, Guid questionId)
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
        public async Task<ActionResult<QuestionDto>> UpdateQuestion(Guid quizzId, Guid questionId, UpdateQuestionRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                Guid userId = GetCurrentUserId();
                var dto = _mapper.Map<QuestionDto>(request);
                if (request.Image != null)
                {
                    var memStream = new MemoryStream();
                    await request.Image.CopyToAsync(memStream);
                    memStream.Position = 0;
                    dto.ImageStream = memStream;
                }
                var response = await _questionService.UpdateAsync(quizzId, questionId, dto, userId);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("{quizzId}/question/{questionId}")]
        public async Task<ActionResult<QuestionDto>> GetQuestionById(Guid quizzId, Guid questionId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var response = await _questionService.GetQuestionById(questionId);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                Exception a = new  Exception(ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPut("{quizzId}/access")]
        public async Task<ActionResult<QuizzAccessDto>> UpdateQuizzAccess(Guid quizzId, UpdateAccessDto updateAccessDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                Guid userId = GetCurrentUserId();
                var response = await _quizzService.UpdateQuizzAccessAsync(quizzId, userId, updateAccessDto);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("{quizzId}/access")]
        public async Task<ActionResult<QuizzAccessDto>> GetQuizzAccess(Guid quizzId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                Guid userId = GetCurrentUserId();
                var response = await _quizzService.GetQuizzAccessAsync(quizzId, userId);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("{quizzId}/groups")]
        public async Task<ActionResult<List<GroupDto>>> GetQuizzGroup(Guid quizzId, [FromQuery] bool shared = true)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                Guid userId = GetCurrentUserId();
                var response = await _groupService.GetQuizzGroupsAsync(userId, quizzId, shared);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("{quizzId}/questions")]
        public async Task<ActionResult<PagedResultDto<QuestionDto>>> GetGroups(
            [FromQuery] QuestionPagedRequestDto request, Guid quizzId
        )
        {
            try
            {
                Guid userId = GetCurrentUserId();
                var result = await _questionService.GetAllQuestionsAsync(quizzId, userId, request.Keyword, request.Newest, request.Page, request.Size);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}
