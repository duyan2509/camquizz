using AutoMapper;
using CamQuizz.Application.Dtos;
using CamQuizz.Domain.Entities;

public class GroupProfile : Profile
{
    public GroupProfile()
    {
        CreateMap<Group, GroupDto>()
            .ForMember(dest=>dest.AmountSharedQuizz, opt => opt.MapFrom(src=>src.QuizzShares.Count))
            .ForMember(dest=>dest.MemberCount, opt=>opt.MapFrom(src=>src.UserGroups.Count))
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
        CreateMap<GroupMessage, MessageDto>()
            .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.Sender, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
            .ForMember(dest => dest.SenderId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message));

        CreateMap<UserGroup, ConversationDto>()
            .ForMember(src => src.GroupName, opt => opt.MapFrom(src => src.Group.Name))
            .ForMember(src => src.GroupId, opt => opt.MapFrom(src => src.Group.Id))
            .ForMember(src => src.LastMessage, opt => opt.MapFrom(src => src.Group.GroupMessages.FirstOrDefault()))
            .ForMember(src => src.UnreadCounts,
                opt => opt.MapFrom(src =>
                    src.LastReadMessageId == null
                        ? -1
                        : src.Group.GroupMessages.Count(ms => ms.CreatedAt > src.LastReadMessage.CreatedAt)));
    }
}
