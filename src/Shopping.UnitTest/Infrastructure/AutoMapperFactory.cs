using AutoMapper;
using Shopping.Core.MappingProfiles;

namespace Shopping.UnitTest.Infrastructure
{
    public static class AutoMapperFactory
    {
        public static IMapper Get()
        {
            var itemMapping = new ItemMapping();
            var shoppingCartMapping = new ShoppingCartMapping();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(itemMapping);
                cfg.AddProfile(shoppingCartMapping);
            });
            return new Mapper(configuration);
        }
    }
}