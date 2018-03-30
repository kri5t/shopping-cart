using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Shopping.Models.Responses;
using Shopping.Proxy.Infrastructure;
using Shopping.Proxy.Infrastructure.Results;

namespace Shopping.Proxy
{
    public interface IShoppingCartProxy
    {
        Task<ModelBaseResult<UidResponse>> CreateShoppingCart();
        Task<ModelBaseResult<List<ShoppingCartResponse>>> ListShoppingCarts();
        Task<BaseResult> DeleteShoppingCart();
        Task<BaseResult> EmptyShoppingCart();
    }

    [UsedImplicitly]
    public class ShoppingCartProxy : IShoppingCartProxy
    {
        private readonly ProxyHttpClient _proxyHttpClient;
        private const string ShoppingCartUri = "api/v1/shoppingcarts";
        public ShoppingCartProxy(ProxyHttpClient proxyHttpClient)
        {
            _proxyHttpClient = proxyHttpClient;
        }

        public async Task<ModelBaseResult<List<ShoppingCartResponse>>> ListShoppingCarts()
        {
            return await _proxyHttpClient.GetAsync<List<ShoppingCartResponse>>(ShoppingCartUri);
        }
        
        public async Task<ModelBaseResult<UidResponse>> CreateShoppingCart()
        {
            return await _proxyHttpClient.PostAsync<UidResponse>(ShoppingCartUri);
        }
        
        public async Task<BaseResult> DeleteShoppingCart()
        {
            return await _proxyHttpClient.DeleteAsync(ShoppingCartUri);
        }
        
        public async Task<BaseResult> EmptyShoppingCart()
        {
            return await _proxyHttpClient.PostAsync(Path.Combine(ShoppingCartUri, "empty"));
        }
    }
}