using CamQuizz.Application.Dtos;
using CamQuizz.Application.Interfaces;
using CamQuizz.Domain;
using CamQuizz.Domain.Entities;
using AutoMapper;
namespace CamQuizz.Application.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;
        public readonly IQuizzRepository _quizzRepository;
        public readonly IQuestionRepository _questionRepository;
        public readonly IAnswerRepository _answerRepository;
        public QuestionService(IUnitOfWork unitOfWork,
            IQuestionRepository questionRepository,
            IAnswerRepository answerRepository,
            IMapper mapper,
            IQuizzRepository quizzRepository)
        {
            _unitOfWork = unitOfWork;
            _quizzRepository = quizzRepository;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _mapper = mapper;
        }

        public async Task<QuestionDto> CreateAsync(CreateQuestionDto createQuestionDto, Guid quizzId, Guid userId)
        {
            var quizz = await _quizzRepository.GetFullByIdAsync(quizzId);
            if (quizz == null)
                throw new InvalidOperationException("Quizz is not found");
            if(quizz.AuthorId != userId)
                throw new InvalidOperationException("Only Author can add new question");

            var questionInfo = new Question
            {
                Content = createQuestionDto.Content,
                Image = createQuestionDto.Image ?? "",
                DurationSecond = createQuestionDto.DurationSecond,
                Point = createQuestionDto.Point,
                QuizzId = quizzId
            };
            var question = await _questionRepository.AddAsync(questionInfo);
            var answers = createQuestionDto.Answers.Select(
                answer => new Answer
                {
                    QuestionId = question.Id,
                    Content = answer.Content,
                    Image = answer.Image ?? "",
                    IsCorrect = answer.IsCorrect,
                }
            ).ToList();
            await _answerRepository.AddRangeAsync(answers);
            return await GetQuestionById(question.Id);
        }
        public async Task<QuestionDto> GetQuestionById(Guid questionId)
        {
            var question = await _questionRepository.GetFullQuestionByIdAsync(questionId);
            if (question == null)
                throw new InvalidOperationException("Question is not found");
            return _mapper.Map<QuestionDto>(question);
        }
        public async Task<bool> DeleteAsync(Guid quizzId, Guid questionId, Guid userId)
        {
            var question = await _questionRepository.GetFullQuestionByIdAsync(questionId);
            if (question == null)
                throw new InvalidOperationException("Question is not found");
            var quizz = await _quizzRepository.GetFullByIdAsync(quizzId);
            if (quizz == null)
                throw new InvalidOperationException("Quizz is not found");
            if(quizz.AuthorId != userId)
                throw new InvalidOperationException("Only Author can delete new question");

            if (quizz.Questions.Count == 1)
                throw new InvalidOperationException("Can't delete the only question on the test");
            await _questionRepository.HardDeleteAsync(questionId);
            return true;
        }
        public async Task<QuestionDto> UpdateAsync(Guid quizzId, QuestionDto questionDto, Guid userId)
        {
            var quizz = await _quizzRepository.GetFullByIdAsync(quizzId);
            if (quizz == null)
                throw new InvalidOperationException("Quizz is not found while update question");
            if (quizz.AuthorId != userId)
                throw new InvalidOperationException("Only Author can update question");

            var question = await _questionRepository.GetFullQuestionByIdAsync(questionDto.Id);
            if (question == null)
                throw new InvalidOperationException("Question is not found");
            if (questionDto.Answers == null)
                throw new InvalidOperationException("Answer list must not be null");
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var currentAnswers = question.Answers.ToList();
                var deletedAnswers = question.Answers.ToList();
                var newAnswers = new List<Answer>();

                foreach (var answerDto in questionDto.Answers)
                {
                    if (answerDto.Id != null)
                    {
                        var existing = currentAnswers.FirstOrDefault(a => a.Id == answerDto.Id);
                        if (existing != null)
                        {
                            deletedAnswers.Remove(existing);
                            if (existing.Content != answerDto.Content ||
                                existing.Image != (answerDto.Image ?? "") ||
                                existing.IsCorrect != answerDto.IsCorrect)
                            {
                                existing.Content = answerDto.Content;
                                existing.Image = answerDto.Image ?? "";
                                existing.IsCorrect = answerDto.IsCorrect;
                            }
                        }
                    }
                    else
                    {
                        newAnswers.Add(new Answer
                        {
                            QuestionId = questionDto.Id,
                            Content = answerDto.Content,
                            Image = answerDto.Image ?? "",
                            IsCorrect = answerDto.IsCorrect
                        });
                    }
                }

                if (deletedAnswers.Any())
                    await _answerRepository.DeleteRangeAsync(deletedAnswers);
                if (newAnswers.Any())
                    await _answerRepository.AddRangeAsync(newAnswers);
                question.Content = questionDto.Content;
                question.Image = questionDto.Image ?? "";
                if (questionDto.DurationSecond != 0)
                    question.DurationSecond = questionDto.DurationSecond;
                if (questionDto.Point != 0)
                    question.Point = questionDto.Point;
                await _questionRepository.UpdateAsync(question);
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

            return await GetQuestionById(questionDto.Id);
        }
        public async Task<Dtos.PagedResultDto<QuestionDto>> GetAllQuestionsAsync(Guid quizzId, Guid userId, int page, int size)
        {
            var quizz = await _quizzRepository.GetFullByIdAsync(quizzId);
            if (quizz == null)
                throw new InvalidOperationException("Quizz is not found");
            if(quizz.AuthorId != userId)
                throw new InvalidOperationException("Only Author can get questions");
            var result = await _questionRepository.GetPagedAsync(page, size);
            var questions = result.Data
                .Select(question => _mapper.Map<QuestionDto>(question))
                .ToList();
            return new Dtos.PagedResultDto<QuestionDto>
            {
                Data = questions,
                Page = result.Page,
                Size = result.Size,
                Total = result.Total,
            };
        }
    }
};







