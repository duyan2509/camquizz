using AutoMapper;
using CamQuizz.Application.Dtos;
using CamQuizz.Domain.Entities;

namespace CamQuizz.Application.Mapping;

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
        CreateMap<QuizzShare, GroupQuizzInfoDto>()
            .ForMember(dest => dest.AuthorName,
                opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
            .ForMember(dest=>dest.Name, opt=>opt.MapFrom(src=>src.Quizz.Name))
            .ForMember(dest=>dest.Image, opt=>opt.MapFrom(src=>src.Quizz.Image))
            .ForMember(dest=>dest.GenreId,  opt=>opt.MapFrom(src=>src.Quizz.GenreId))
            .ForMember(dest=>dest.AuthorId,  opt=>opt.MapFrom(src=>src.Quizz.AuthorId))
            .ForMember(dest => dest.GenreName, opt => opt.MapFrom(src => src.Quizz.Genre.Name))
            .ForMember(dest => dest.NumberOfQuestions,
                opt => opt.MapFrom(src => src.Quizz.Questions.Count))
            .ForMember(dest => dest.ShareAt, opt => opt.MapFrom(src => src.CreatedAt));

        CreateMap<Quizz, QuizzDto>()
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => $"{src.Author.FirstName} {src.Author.LastName}"))
            .ForMember(dest => dest.GenreName, opt => opt.MapFrom(src => src.Genre.Name))
            .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt ?? src.CreatedAt));

        CreateMap<Quizz, QuizzAccessDto>()
            .ForMember(dest => dest.Accesses, opt => opt.MapFrom(quiz => quiz.QuizzShares));
        CreateMap<QuizzShare, AccessDto>()
            .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.Group.Name))
            .ForMember(dest=>dest.ShareAt, opt=>opt.MapFrom(src=>src.CreatedAt));
    }
    
}