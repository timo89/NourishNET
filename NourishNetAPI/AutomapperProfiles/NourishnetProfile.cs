using AutoMapper;
using NourishNet.Domain.Entities;
using NourishNetAPI.DTO.Beneficiary;

namespace NourishNetAPI.AutomapperProfiles
{
    public class NourishnetProfile : Profile
    {
        public NourishnetProfile()
        {
            CreateMap<Beneficiary, BeneficiaryDTO>()
                 .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name));
        }
    }
}
