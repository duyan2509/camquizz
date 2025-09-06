using AutoMapper;
using CamQuizz.Application.Dtos;
using CamQuizz.Domain.Entities;
using NanoidDotNet;

public class GameProfile : Profile
{
    public GameProfile()
    {
        CreateMap<User, Player>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
    }
}
