using ATEC_API.Data.DTO;
using ATEC_API.DomainModels.HRISDomainModel;
using AutoMapper;

namespace ATEC_API.MappingProfile
{
    public class HRISMappingProfile : Profile
    {
        public HRISMappingProfile()
        {
            CreateMap<TblCert, HRISDTO>().ReverseMap();
        }
    }
}