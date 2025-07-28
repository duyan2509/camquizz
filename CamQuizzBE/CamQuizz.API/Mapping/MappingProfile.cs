using AutoMapper;
using CamQuizz.Application.Dtos;
using CamQuizz.Request;

namespace CamQuizz.Mapping;

public class MappingProfile:Profile
{
    public MappingProfile()
    {
        CreateMap<CreateQuizzRequest, CreateQuizzDto>()
            .ForMember(dest => dest.ImageStream, opt => opt.Ignore())
            .ForMember(dest=>dest.Image, opt=>opt.MapFrom(src=>""));
        CreateMap<CreateQuestionRequest, CreateQuestionDto>()
            .ForMember(dest => dest.ImageStream, opt => opt.Ignore())
            .ForMember(dest=>dest.Image, opt=>opt.MapFrom(src=>""));
        CreateMap<UpdateInfoRequest, UpdateQuizzDto>()
            .ForMember(dest => dest.ImageStream, opt => opt.Ignore())
            .ForMember(dest=>dest.Image, opt=>opt.MapFrom(src=>""));
        CreateMap<UpdateQuestionRequest, QuestionDto>()
            .ForMember(dest => dest.ImageStream, opt => opt.Ignore())
            .ForMember(dest=>dest.Image, opt=>opt.MapFrom(src=>""));

    }
}