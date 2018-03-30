using AutoMapper;
using JetBrains.Annotations;
using Shopping.Database.Models;
using Shopping.Models.Responses;

namespace Shopping.Core.MappingProfiles
{
    [UsedImplicitly]
    public class ItemMapping : Profile
    {
        public ItemMapping()
        {
            CreateMap<Item, ItemResponse>()
                .ForMember(res => res.CreatedDate, conf => conf.MapFrom(item => item.CreatedDate))
                .ForMember(res => res.UpdatedDate, conf => conf.MapFrom(item => item.UpdatedDate))
                .ForMember(res => res.Description, conf => conf.MapFrom(item => item.Description))
                .ForMember(res => res.Quantity, conf => conf.MapFrom(item => item.Quantity))
                .ForMember(res => res.Uid, conf => conf.MapFrom(item => item.Uid));
        }
    }
}