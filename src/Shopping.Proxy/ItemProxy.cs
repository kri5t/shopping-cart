using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Shopping.Models.Responses;
using Shopping.Proxy.Infrastructure;
using Shopping.Proxy.Infrastructure.Results;

namespace Shopping.Proxy
{
    public interface IItemProxy
    {
        Task<ModelBaseResult<UidResponse>> CreateItem(Guid shoppingCartUid, string description, int quantity);
        Task<BaseResult> DeleteItem(Guid shoppingCartUid);
    }

    [UsedImplicitly]
    public class ItemProxy : IItemProxy
    {
        private readonly ProxyHttpClient _proxyHttpClient;
        private string ItemUri(Guid shoppingCartUid) => $"api/v1/shoppingcarts/{shoppingCartUid}/items";
        
        public ItemProxy(ProxyHttpClient proxyHttpClient)
        {
            _proxyHttpClient = proxyHttpClient;
        }
        
        public async Task<ModelBaseResult<UidResponse>> CreateItem(Guid shoppingCartUid, string description, int quantity)
        {
            return await _proxyHttpClient.PostAsync<UidResponse>(ItemUri(shoppingCartUid), new {description, quantity});
        }
        
        public async Task<BaseResult> DeleteItem(Guid shoppingCartUid)
        {
            return await _proxyHttpClient.DeleteAsync(ItemUri(shoppingCartUid));
        }
    }
}