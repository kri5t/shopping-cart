using AutoMapper;
using JetBrains.Annotations;
using Shopping.Database.Models;
using Shopping.Models.Responses;

namespace Shopping.Core.MappingProfiles
{
    [UsedImplicitly]
    public class ShoppingCartMapping : Profile
    {
        public ShoppingCartMapping()
        {
            CreateMap<ShoppingCart, ShoppingCartResponse>()
                .ForMember(res => res.CreatedDate, conf => conf.MapFrom(sc => sc.CreatedDate))
                .ForMember(res => res.Uid, conf => conf.MapFrom(sc => sc.Uid))
                .ForMember(res => res.ItemList, conf => conf.MapFrom(sc => sc.Items))
                .ForMember(res => res.UpdatedDate, conf => conf.MapFrom(sc => sc.UpdatedDate));
        }
    }
}