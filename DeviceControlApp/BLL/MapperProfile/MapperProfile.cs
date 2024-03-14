using AutoMapper;
using DeviceControlApp.BLL.DTO;

namespace DeviceControlApp.BLL.MapperProfile;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<DAL.Entities.Device, DeviceEditDto>();
        CreateMap<DeviceEditDto, DAL.Entities.Device>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        
        CreateMap<DAL.Entities.Device, DeviceAddDto>(); 
        CreateMap<DeviceAddDto, DAL.Entities.Device>()  
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}