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
        public readonly ICloudStorageService _storageService;
        public QuestionService(IUnitOfWork unitOfWork,
            IQuestionRepository questionRepository,
            IAnswerRepository answerRepository,
            IMapper mapper,
            IQuizzRepository quizzRepository,
            ICloudStorageService storageService)

        {
            _unitOfWork = unitOfWork;
            _quizzRepository = quizzRepository;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _mapper = mapper;
            _storageService = storageService;
        }

        public async Task<QuestionDto> CreateAsync(CreateQuestionDto createQuestionDto, Guid quizzId, Guid userId)
        {
            var quizz = await _quizzRepository.GetFullByIdAsync(quizzId);
            if (quizz == null)
                throw new InvalidOperationException("Quizz is not found");
            if(quizz.AuthorId != userId)
                throw new InvalidOperationException("Only Author can add new question");
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                if (createQuestionDto.ImageStream != null)
                    createQuestionDto.Image =
                        await _storageService.UploadAsync(createQuestionDto.ImageStream, Guid.NewGuid().ToString());
                var questionInfo = new Question
                {
                    Content = createQuestionDto.Content,
                    Image = createQuestionDto.Image ?? "",
                    DurationSecond = createQuestionDto.DurationSecond,
                    Point = createQuestionDto.Point,
                    QuizzId = quizzId
                };
                var question = await _questionRepository.AddAsync(questionInfo);
                int trueAnswers = 0;
                var answers = createQuestionDto.Answers.Select(
                    answer =>
                    {
                        if(answer.IsCorrect) trueAnswers++;
                        if(trueAnswers == 2)
                            throw new InvalidOperationException("Question must be 1 true answer");
                        return new Answer
                        {
                            QuestionId = question.Id,
                            Content = answer.Content,
                            Image = answer.Image ?? "",
                            IsCorrect = answer.IsCorrect,
                        };
                    }).ToList();
                await _answerRepository.AddRangeAsync(answers);
                await _unitOfWork.CommitAsync();
                return await GetQuestionById(question.Id);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
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
            var quizz = await _quizzRepository.GetByIdAsync(quizzId);
            if (quizz == null)
                throw new InvalidOperationException("Quizz is not found");
            if(quizz.AuthorId != userId)
                throw new InvalidOperationException("Only Author can delete question");
            var quizQuestionCount = await _questionRepository.GetQuestionCountByQuizIdAsync(quizzId);
            if (quizQuestionCount == 1)
                throw new InvalidOperationException("Can't delete the only question on the test");
            if(!string.IsNullOrEmpty(question.Image))
                await  _storageService.DeleteAsync(question.Image);
            await _questionRepository.HardDeleteAsync(questionId);
            return true;
        }
        public async Task<QuestionDto> UpdateAsync(Guid quizzId, Guid questionId, QuestionDto questionDto, Guid userId)
        {
            var quizz = await _quizzRepository.GetByIdAsync(quizzId);
            if (quizz == null)
                throw new InvalidOperationException("Quizz is not found while update question");
            if (quizz.AuthorId != userId)
                throw new InvalidOperationException("Only Author can update question");

            var question = await _questionRepository.GetFullQuestionByIdAsync(questionId);
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
                if (!string.IsNullOrEmpty(question.Image) && questionDto.ImageStream != null)
                {
                    await _storageService.DeleteAsync(question.Image);
                    question.Image =
                        await _storageService.UploadAsync(questionDto.ImageStream, Guid.NewGuid().ToString());
                }
                else if(questionDto.ImageStream != null)
                    question.Image= await _storageService.UploadAsync(questionDto.ImageStream, Guid.NewGuid().ToString());
                if (questionDto.DurationSecond != 0)
                    question.DurationSecond = questionDto.DurationSecond;
                if (questionDto.Point != 0)
                    question.Point = questionDto.Point;
                await _questionRepository.UpdateAsync(question);
                await _unitOfWork.CommitAsync();
                return await GetQuestionById(questionDto.Id);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

        }
        public async Task<Dtos.PagedResultDto<QuestionDto>> GetAllQuestionsAsync(
            Guid quizzId, 
            Guid userId, 
            string? kw,
            bool newest, 
            int page, 
            int size)
        {
            var quizz = await _quizzRepository.GetFullByIdAsync(quizzId);
            if (quizz == null)
                throw new InvalidOperationException("Quizz is not found");
            if(quizz.AuthorId != userId)
                throw new InvalidOperationException("Only Author can get questions");
            var result = await _questionRepository.GetQuestionsByQuizIdAsync(quizzId, kw, newest, page, size);
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







