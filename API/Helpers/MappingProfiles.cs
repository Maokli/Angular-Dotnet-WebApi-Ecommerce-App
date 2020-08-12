using API.DTOs;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
  public class MappingProfiles : Profile
  {
    public MappingProfiles()
    {
        CreateMap<Product,ProductToReturnDTO>()
        .ForMember(d => d.ProductBrand, o => o.MapFrom(p => p.ProductBrand.Name))
        .ForMember(d => d.ProductType, o => o.MapFrom(p => p.ProductType.Name))
        .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());
    }
  }
}