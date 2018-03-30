using System;
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
        Task<ModelBaseResult<List<ShoppingCartResponse>>> ListShoppingCarts(bool includeItems = false);
        Task<BaseResult> DeleteShoppingCart(Guid uid);
        Task<BaseResult> EmptyShoppingCart(Guid uid);
    }

    [UsedImplicitly]
    public class ShoppingCartProxy : IShoppingCartProxy
    {
        private readonly ProxyHttpClient _proxyHttpClient;
        private const string ShoppingCartUri = "api/v1/shoppingcarts";
        private string ShoppingCartUriWithUid(Guid uid) => $"api/v1/shoppingcarts/{uid}";
        
        public ShoppingCartProxy(ProxyHttpClient proxyHttpClient)
        {
            _proxyHttpClient = proxyHttpClient;
        }

        public async Task<ModelBaseResult<List<ShoppingCartResponse>>> ListShoppingCarts(bool includeItems = false)
        {
            return await _proxyHttpClient.GetAsync<List<ShoppingCartResponse>>(ShoppingCartUri, 
                new Dictionary<string, string>
                {
                    {nameof(includeItems), includeItems.ToString()}
                });
        }
        
        public async Task<ModelBaseResult<UidResponse>> CreateShoppingCart()
        {
            return await _proxyHttpClient.PostAsync<UidResponse>(ShoppingCartUri);
        }
        
        public async Task<BaseResult> DeleteShoppingCart(Guid uid)
        {
            return await _proxyHttpClient.DeleteAsync(ShoppingCartUriWithUid(uid));
        }
        
        public async Task<BaseResult> EmptyShoppingCart(Guid uid)
        {
            return await _proxyHttpClient.PostAsync(Path.Combine(ShoppingCartUriWithUid(uid), "empty"));
        }
    }
}