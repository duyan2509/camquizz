using AutoMapper;
using CamQuizz.Application.Dtos;
using CamQuizz.Domain.Entities;

public class QuizzProfile : Profile
{
    public QuizzProfile()
    {
        CreateMap<Answer, AnswerDto>();
        CreateMap<Question, QuestionDto>()
            .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers));
        CreateMap<Quizz, QuizzInfoDto>()
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => $"{src.Author.FirstName} {src.Author.LastName}"))
            .ForMember(dest => dest.GenreName, opt => opt.MapFrom(src => src.Genre.Name))
            .ForMember(dest => dest.NumberOfQuestions, opt => opt.MapFrom(src => src.Questions != null ? src.Questions.Count : 0))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt ?? src.CreatedAt));

        CreateMap<Quizz, QuizzDto>()
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => $"{src.Author.FirstName} {src.Author.LastName}"))
            .ForMember(dest => dest.GenreName, opt => opt.MapFrom(src => src.Genre.Name))
            .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt ?? src.CreatedAt));
    }
}
