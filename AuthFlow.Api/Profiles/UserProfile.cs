using AutoMapper;

namespace AuthFlow.Api.Profiles
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<Domain.Entities.User, Domain.DTO.User>().ReverseMap();
        }
    }
}