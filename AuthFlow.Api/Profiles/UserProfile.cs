namespace AuthFlow.Api.Profiles
{
    using AutoMapper;
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<Domain.Entities.User, Domain.DTO.User>().ReverseMap();
        }
    }
}