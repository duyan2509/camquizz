using AutoMapper;
using CamQuizz.Application.Dtos;
using CamQuizz.Domain.Entities;

public class GroupProfile : Profile
{
    public GroupProfile()
    {
        CreateMap<Group, GroupDto>()
            .ForMember(dest=>dest.AmountSharedQuizz, opt => opt.MapFrom(src=>src.QuizzShares!=null?src.QuizzShares.Count:0))
            .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => $"{src.Owner.FirstName} {src.Owner.LastName}"));
        CreateMap<Group, FullGroupDto>()
            .ForMember(dest => dest.OwnerName,
                opt => opt.MapFrom(src => $"{src.Owner.FirstName} {src.Owner.LastName}"))
            .ForMember(dest => dest.Quizzes,
                opt => opt.MapFrom(src => src.QuizzShares.Select(qs => qs.Quizz)))
            .ForMember(dest => dest.Members,
                opt => opt.MapFrom(src => src.UserGroups.Select(ug => ug.User)));

        CreateMap<User, MemberDto>()
            .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
        CreateMap<UserGroup, UserGroupDto>();
    }
}
