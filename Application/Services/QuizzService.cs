using CamQuizz.Application.Dtos;
using CamQuizz.Application.Interfaces;
using CamQuizz.Domain;
using CamQuizz.Domain.Entities;
using CamQuizz.Persistence;
using CamQuizz.Persistence.Interfaces;
using CamQuizz.Persistence.Repositories;
using Humanizer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using AutoMapper;
namespace CamQuizz.Application.Services
{
    public class QuizzService : IQuizzService
    {
        private readonly IMapper _mapper;

        private readonly ApplicationDbContext _context;
        public readonly IQuizzRepository _quizzRepository;
        public readonly IQuestionRepository _questionRepository;
        public readonly IAnswerRepository _answerRepository;
        public readonly IUserRepository _userRepository;
        public readonly IGenreRepository _genreRepository;

        public QuizzService(ApplicationDbContext context,
            IQuizzRepository quizzRepository,
            IQuestionRepository questionRepository,
            IAnswerRepository answerRepository,
            IMapper mapper,
            IUserRepository userRepository,
            IGenreRepository genreRepository)
        {
            _context = context;
            _quizzRepository = quizzRepository;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _genreRepository = genreRepository;

        }
        public async Task<QuizzDto> CreateAsync(CreateQuizzDto createQuizzDto)
        {
            Guid createdQuizzId;
            var genre = await _genreRepository.GetByIdAsync(createQuizzDto.GenreId);
            if (genre == null)
                throw new InvalidOperationException("Genre does not exist");
            if (!Enum.IsDefined(typeof(QuizzStatus), createQuizzDto.Status))
            {
                throw new InvalidOperationException("Invalid Quizz Status");
            }
            var author = await _userRepository.GetByIdAsync(createQuizzDto.AuthorId);
            if (author == null)
                throw new InvalidOperationException("User does not exist");

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var quizInfo = new Quizz
                {
                    Name = createQuizzDto.Name,
                    Image = createQuizzDto.Image,
                    AuthorId = createQuizzDto.AuthorId,
                    GenreId = createQuizzDto.GenreId
                };
                var quizz = await _quizzRepository.AddAsync(quizInfo);
                foreach (var createQuestion in createQuizzDto.Questions)
                {
                    Question questionInfo = new Question
                    {
                        QuizzId = quizz.Id,
                        Content = createQuestion.Content,
                        DurationSecond = createQuestion.DurationSecond,
                        Point = createQuestion.Point
                    };
                    var question = await _questionRepository.AddAsync(questionInfo);
                    var answers = createQuestion.Answers.Select(dto => new Answer
                    {
                        Content = dto.Content,
                        Image = dto.Image,
                        IsCorrect = dto.IsCorrect,
                        QuestionId = question.Id,
                    }).ToList();
                    await _answerRepository.AddRangeAsync(answers);
                }


                await transaction.CommitAsync();
                createdQuizzId = quizz.Id;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
            return await GetFullQuizzByIdAsync(createdQuizzId);


        }

        public async Task<QuizzDto> GetFullQuizzByIdAsync(Guid id)
        {
            var quizz = await _quizzRepository.GetFullByIdAsync(id);
            if (quizz == null)
                throw new InvalidOperationException("Quizz is not found.");
            return _mapper.Map<QuizzDto>(quizz);
        }

        public async Task<QuizzInfoDto> GetQuizInfoByIdAsync(Guid id)
        {
            var quizz = await _quizzRepository.GetInfoByIdAsync(id);
            if (quizz == null)
                throw new InvalidOperationException("Quizz is not found.");
            return _mapper.Map<QuizzInfoDto>(quizz);
        }
        public async Task<PagedResultDto<QuizzInfoDto>> GetMyQuizzesAsync(
            int pageNumber,
            int pageSize,
            QuizzStatus? quizzStatus,
            Guid userId)
        {
            var result = await _quizzRepository.GetMyQuizzesAsync(pageNumber, pageSize, quizzStatus, userId);
            var quizzes = result.Data
                .Select(quizz => _mapper.Map<QuizzInfoDto>(quizz))
                .ToList();
            return new PagedResultDto<QuizzInfoDto>
            {
                Data = quizzes,
                Page = result.Page,
                Total = result.Total,
                Size = result.Size
            };
        }
        public async Task<bool> DeleteQuiz(Guid id)
        {
            var quizz = await _quizzRepository.GetInfoByIdAsync(id);
            if (quizz == null)
                throw new InvalidOperationException("Quizz is not found.");
            await _quizzRepository.HardDeleteAsync(id);
            return true;
        }
        public async Task<QuizzInfoDto> UpdateQuizInfoAsync(Guid id, UpdateQuizzDto updateQuizzDto)
        {
            var quizz = await _quizzRepository.GetInfoByIdAsync(id);
            if (quizz == null)
                throw new InvalidOperationException("Quizz is not found");
            if (updateQuizzDto.GenreId.HasValue)
            {
                var genre = await _genreRepository.GetByIdAsync(updateQuizzDto.GenreId.Value);
                if (genre == null)
                    throw new InvalidOperationException("Genre does not exist");

                quizz.GenreId = updateQuizzDto.GenreId.Value;
            }
            if (updateQuizzDto.Status.HasValue &&
                !Enum.IsDefined(typeof(QuizzStatus), updateQuizzDto.Status.Value))
            {
                throw new InvalidOperationException("Invalid Quizz Status");
            }


            quizz.Name = updateQuizzDto.Name ?? quizz.Name;
            quizz.Image = updateQuizzDto.Image ?? quizz.Image;
            quizz.Status = updateQuizzDto.Status ?? quizz.Status;
            quizz.UpdatedAt = DateTime.UtcNow;
            await _quizzRepository.UpdateAsync(quizz);
            return await GetQuizInfoByIdAsync(id);
        }
    }
}
