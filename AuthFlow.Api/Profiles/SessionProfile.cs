using AutoMapper;

namespace AuthFlow.Api.Profiles
{
    public class SessionProfile : Profile
    {
        public SessionProfile()
        {
            CreateMap<Domain.Entities.Session, Domain.DTO.Session>().ReverseMap();
        }
    }
}