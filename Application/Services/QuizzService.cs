using CamQuizz.Application.Dtos;
using CamQuizz.Application.Interfaces;
using CamQuizz.Domain;
using CamQuizz.Domain.Entities;
using CamQuizz.Persistence;
using CamQuizz.Persistence.Interfaces;
using CamQuizz.Persistence.Repositories;
using Humanizer;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CamQuizz.Application.Services
{
    public class QuizzService : IQuizzService
    {
        private readonly ApplicationDbContext _context;
        public readonly IQuizzRepository _quizzRepository;
        public readonly IQuestionRepository _questionRepository;
        public readonly IAnswerRepository _answerRepository;
        public QuizzService (ApplicationDbContext context, IQuizzRepository quizzRepository, IQuestionRepository questionRepository, IAnswerRepository answerRepository)
        {
            _context = context;
            _quizzRepository = quizzRepository;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
        }
        public async Task<QuizzDto> CreateAsync(CreateQuizzDto createQuizzDto)
        {
            Guid createdQuizzId;
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
            return new QuizzDto
            {
                Id = quizz.Id,
                Name = quizz.Name,
                Image = quizz.Image,
                GenreId = quizz.GenreId ?? Guid.Empty,
                AuthorId = quizz.AuthorId,
                AuthorName = $"{quizz.Author.FirstName} {quizz.Author.LastName}",
                GenreName = quizz.Genre.Name,
                Status = quizz.Status,
                Questions = quizz.Questions.Select(q => new QuestionDto
                {
                    Id = q.Id,
                    Content = q.Content,
                    DurationSecond = q.DurationSecond,
                    Point = q.Point,
                    Image = q.Image,
                    Answers = q.Answers.Select(a => new AnswerDto
                    {
                        Id = a.Id,
                        Content = a.Content,
                        IsCorrect = a.IsCorrect,
                        Image = a.Image
                    }).ToList()
                }).ToList(),

                NumberOfAttemps = quizz.NumberOfAttemps,
                CreatedAt = quizz.CreatedAt,
                UpdatedAt = quizz.UpdatedAt ?? quizz.CreatedAt
            };
        }

        public async Task<QuizzInfoDto> GetQuizInfoByIdAsync(Guid id)
        {
            var quizz = await _quizzRepository.GetInfoByIdAsync(id);
            if (quizz == null)
                throw new InvalidOperationException("Quizz is not found.");
            return new QuizzInfoDto
            {
                Id = quizz.Id,
                Name = quizz.Name,
                Image = quizz.Image,
                GenreId = quizz.GenreId ?? Guid.Empty,
                AuthorId = quizz.AuthorId,
                AuthorName = $"{quizz.Author.FirstName} {quizz.Author.LastName}",
                GenreName = quizz.Genre.Name,
                Status = quizz.Status,
                NumberOfAttemps = quizz.NumberOfAttemps,
                CreatedAt = quizz.CreatedAt,
                UpdatedAt = quizz.UpdatedAt ?? quizz.CreatedAt,
                NumberOfQuestions = quizz.Questions?.Count ?? 0
            };
        }
    }
}
