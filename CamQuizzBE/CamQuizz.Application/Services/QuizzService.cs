using System.Runtime.InteropServices;
using CamQuizz.Application.Dtos;
using CamQuizz.Application.Interfaces;
using CamQuizz.Domain;
using CamQuizz.Domain.Entities;
using AutoMapper;
using CamQuizz.Application.Exceptions;

namespace CamQuizz.Application.Services
{
    public class QuizzService : IQuizzService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public readonly IQuizzRepository _quizzRepository;
        public readonly IQuestionRepository _questionRepository;
        public readonly IAnswerRepository _answerRepository;
        public readonly IUserRepository _userRepository;
        public readonly IGenreRepository _genreRepository;
        public readonly IGroupRepository _groupRepository;
        public readonly IQuizzShareRepository _quizzShareRepository;
        public readonly ICloudStorageService _storageService;
        public QuizzService(
            IQuizzRepository quizzRepository,
            IQuestionRepository questionRepository,
            IAnswerRepository answerRepository,
            IMapper mapper,
            IUserRepository userRepository,
            IGenreRepository genreRepository,
            IGroupRepository groupRepository,
            IQuizzShareRepository quizzShareRepository,
            IUnitOfWork unitOfWork,
            ICloudStorageService storageService)
        {
            _quizzRepository = quizzRepository;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _genreRepository = genreRepository;
            _groupRepository = groupRepository;
            _quizzShareRepository = quizzShareRepository;
            _unitOfWork = unitOfWork;
            _storageService = storageService;
        }
        public async Task<QuizzDto> CreateAsync(Guid authorId, CreateQuizzDto createQuizzDto)
        {

            if (!Enum.IsDefined(typeof(QuizzStatus), createQuizzDto.Status))
            {
                throw new InvalidOperationException("Invalid Quizz Status");
            }
            Guid createdQuizzId;
            var genre = await _genreRepository.GetByIdAsync(createQuizzDto.GenreId);
            if (genre == null)
                throw new InvalidOperationException("Genre does not exist");

            var author = await _userRepository.GetByIdAsync(authorId);
            if (author == null)
                throw new InvalidOperationException("User does not exist");

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                if (createQuizzDto.ImageStream != null)
                    createQuizzDto.Image =
                        await _storageService.UploadAsync(createQuizzDto.ImageStream, Guid.NewGuid().ToString());
                var quizInfo = new Quizz
                {
                    Name = createQuizzDto.Name,
                    Image = createQuizzDto.Image,
                    AuthorId = authorId,
                    GenreId = createQuizzDto.GenreId
                };
                var quizz = await _quizzRepository.AddAsync(quizInfo);
                if (createQuizzDto.Questions != null)
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
                        var countTrueAnswers = 0;
                        foreach (var createAnswerDto in createQuestion.Answers)
                        {
                            countTrueAnswers = createAnswerDto.IsCorrect ? countTrueAnswers + 1 : countTrueAnswers;
                            if (countTrueAnswers == 2)
                                throw new InvalidOperationException(
                                    $"Question {createQuestion.Content} must be have only 1 true answer");
                        }

                        var answers = createQuestion.Answers.Select(dto => new Answer
                        {
                            Content = dto.Content,
                            Image = dto.Image,
                            IsCorrect = dto.IsCorrect,
                            QuestionId = question.Id,
                        }).ToList();
                        await _answerRepository.AddRangeAsync(answers);
                    }
                await UpdateQuizzAccessAsync(quizz.Id, authorId, new UpdateAccessDto
                {
                    Status = createQuizzDto.Status,
                    GroupIds = createQuizzDto.GroupIds
                });
                await _unitOfWork.CommitAsync();
                return await GetFullQuizzByIdAsync(quizz.Id);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

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

        public async Task<DetailQuizDto> GetDetailByIdAsync(Guid id)
        {
            var quizz = await _quizzRepository.GetDetailByIdAsync(id);
            if (quizz == null)
                throw new InvalidOperationException("Quizz is not found.");
            return _mapper.Map<DetailQuizDto>(quizz);
        }

        public async Task<PagedResultDto<QuizzInfoDto>> GetAllQuizzesAsync(
            string? kw,
            Guid? categoryId,
            bool popular,
            bool newest,
            int pageNumber,
            int pageSize)
        {
            var result = await _quizzRepository.GetAllQuizzesAsync(kw, categoryId, popular, newest, pageNumber, pageSize);
            var quizzes = result.Data
                .Select(quizz => _mapper.Map<QuizzInfoDto>(quizz))
                .ToList();
            return new Dtos.PagedResultDto<QuizzInfoDto>
            {
                Data = quizzes,
                Page = result.Page,
                Total = result.Total,
                Size = result.Size
            };
        }
        public async Task<PagedResultDto<QuizzInfoDto>> GetMyQuizzesAsync(
            string? kw,
            Guid? categoryId,
            bool popular,
            bool newest,
            int pageNumber,
            int pageSize,
            QuizzStatus? quizzStatus,
            Guid userId)
        {
            var result = await _quizzRepository.GetMyQuizzesAsync(kw, categoryId, popular, newest, pageNumber, pageSize, quizzStatus, userId);
            var quizzes = result.Data
                .Select(quizz => _mapper.Map<QuizzInfoDto>(quizz))
                .ToList();
            return new Dtos.PagedResultDto<QuizzInfoDto>
            {
                Data = quizzes,
                Page = result.Page,
                Total = result.Total,
                Size = result.Size
            };
        }
        public async Task<bool> DeleteQuiz(Guid userId, Guid id)
        {
            var quizz = await _quizzRepository.GetFullByIdAsync(id);
            if (quizz == null)
                throw new InvalidOperationException("Quizz is not found.");
            if(quizz.AuthorId != userId)
                throw new ForbiddenException("Only Author can delete question");
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                if(!string.IsNullOrEmpty(quizz.Image))
                    await _storageService.DeleteAsync(quizz.Image);
                var questionImgs = quizz.Questions.Select(question => question.Image).Where(img=>!string.IsNullOrEmpty(img)).ToList();
                var deleteTasks = questionImgs.Select(img => _storageService.DeleteAsync(img));
                await Task.WhenAll(deleteTasks);
                var quizzShares = await _quizzShareRepository.GetByQuizzIdAsync(quizz.Id);
                if(quizzShares!=null)
                    await _quizzShareRepository.DeleteRangeAsync(quizzShares);
                await _quizzRepository.HardDeleteAsync(id);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new InvalidDataException(ex.Message);
            }
            
        }





        public async Task<QuizzInfoDto> UpdateQuizInfoAsync(Guid id, UpdateQuizzDto updateQuizzDto, Guid userId)
        {

            var quizz = await _quizzRepository.GetInfoByIdAsync(id);
            if (quizz == null)
                throw new InvalidOperationException("Quizz is not found");
            if (quizz.AuthorId != userId)
                throw new UnauthorizedAccessException("You are not allowed to update this quizz.");

            if (updateQuizzDto.ImageStream != null && !string.IsNullOrEmpty(quizz.Image))
            {
                await _storageService.DeleteAsync(quizz.Image);
                quizz.Image = await _storageService.UploadAsync(updateQuizzDto.ImageStream, Guid.NewGuid().ToString());
            }
            else if(updateQuizzDto.ImageStream != null)
                quizz.Image = await _storageService.UploadAsync(updateQuizzDto.ImageStream, Guid.NewGuid().ToString());
                
            if (updateQuizzDto.GenreId.HasValue)
            {
                var genre = await _genreRepository.GetByIdAsync(updateQuizzDto.GenreId.Value);
                if (genre == null)
                    throw new InvalidOperationException("Genre does not exist");

                quizz.GenreId = updateQuizzDto.GenreId.Value;
            }
            quizz.Name = updateQuizzDto.Name ?? quizz.Name;
            quizz.UpdatedAt = DateTime.UtcNow;
            await _quizzRepository.UpdateAsync(quizz);
            return await GetQuizInfoByIdAsync(id);
        }

        public async Task<QuizzAccessDto> GetQuizzAccessAsync(Guid quizzId, Guid userId)
        {
            var quizz = await _quizzRepository.GetAccessQuizzByIdAsync(quizzId);
            if (quizz == null)
                throw new InvalidOperationException("Quizz is not found.");
            if (quizz.AuthorId != userId)
                throw new UnauthorizedAccessException("Only author can view access to quizz.");
            return _mapper.Map<QuizzAccessDto>(quizz);
        }
        public async Task<QuizzAccessDto> UpdateQuizzAccessAsync(Guid quizzId, Guid userId, UpdateAccessDto updateAccessDto)
        {
            var quizz = await _quizzRepository.GetAccessQuizzByIdAsync(quizzId);
            if (quizz == null)
                throw new InvalidOperationException("Quizz is not found.");
            if (quizz.AuthorId != userId)
                throw new UnauthorizedAccessException("Only author can update access to quizz.");
            if (!Enum.IsDefined(typeof(QuizzStatus), updateAccessDto.Status))
                throw new InvalidOperationException("Invalid Quizz Status, must be Public or Private whith Groups");
            if (updateAccessDto is { Status: QuizzStatus.Private, GroupIds.Count: 0 })
                throw new InvalidOperationException("Private quizz must have at least one group.");
            if (updateAccessDto.Status == QuizzStatus.Public && updateAccessDto.GroupIds.Count > 0)
                throw new InvalidOperationException("Public quizz must have no groups.");
            var myGroups = await _groupRepository.GetAllGroupsAsync(userId);
            var myGroupIds = new HashSet<Guid>(myGroups.Select(g => g.Id));
            var newGroupIds = updateAccessDto.GroupIds;
            if (!newGroupIds.All(g => myGroupIds.Contains(g)))
                throw new InvalidOperationException("You are not alowed to share this quizz with some groups.");
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var currentQuizzShares = quizz.QuizzShares.ToList();
                var currentGroupIds = currentQuizzShares.Select(x => x.GroupId).ToHashSet();

                var toAdd = newGroupIds.Except(currentGroupIds).ToList();
                var toRemove = currentQuizzShares
                    .Where(qs => !newGroupIds.Contains(qs.GroupId))
                    .ToList();
                var sharesToAdd = toAdd.Select(gid => new QuizzShare
                {
                    GroupId = gid,
                    QuizzId = quizzId,
                    UserId = userId,
                    UpdatedAt = DateTime.UtcNow
                }).ToList();

                await _quizzShareRepository.AddRangeAsync(sharesToAdd);
                await _quizzShareRepository.DeleteRangeAsync(toRemove);
                await _quizzRepository.UpdateStatusASync(quizz, updateAccessDto.Status);
                await _unitOfWork.CommitAsync();
                return await GetQuizzAccessAsync(quizzId, userId);
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackAsync();
                Console.WriteLine(e);
                throw new InvalidOperationException($"Error when update access:{e.Message} ");
            }

        }

    }
}
