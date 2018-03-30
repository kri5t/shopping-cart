using System;
using System.Collections.Generic;
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
        Task<BaseResult> UpdateItem(Guid shoppingCartUid, Guid itemUid, string description, int quantity);
        Task<ModelBaseResult<List<ItemResponse>>> ListItems(Guid shoppingCartUid);
        Task<ModelBaseResult<ItemResponse>> GetItem(Guid shoppingCartUid, Guid itemUid);
        Task<BaseResult> DeleteItem(Guid shoppingCartUid, Guid itemUid);
    }

    [UsedImplicitly]
    public class ItemProxy : IItemProxy
    {
        private readonly ProxyHttpClient _proxyHttpClient;
        private string ItemUri(Guid shoppingCartUid) => $"api/v1/shoppingcarts/{shoppingCartUid}/items";
        private string ItemUriWithUid(Guid shoppingCartUid, Guid itemUid) 
            => $"{ItemUri(shoppingCartUid)}/{itemUid}";
        
        public ItemProxy(ProxyHttpClient proxyHttpClient)
        {
            _proxyHttpClient = proxyHttpClient;
        }
        
        public async Task<ModelBaseResult<UidResponse>> CreateItem(Guid shoppingCartUid, string description, int quantity)
        {
            return await _proxyHttpClient.PostAsync<UidResponse>(ItemUri(shoppingCartUid), new {description, quantity});
        }
        
        public async Task<BaseResult> UpdateItem(Guid shoppingCartUid, Guid itemUid, string description, int quantity)
        {
            return await _proxyHttpClient.PutAsync(ItemUriWithUid(shoppingCartUid, itemUid), new {description, quantity});
        }
        
        public async Task<ModelBaseResult<List<ItemResponse>>> ListItems(Guid shoppingCartUid)
        {
            return await _proxyHttpClient.GetAsync<List<ItemResponse>>(ItemUri(shoppingCartUid));
        }
        
        public async Task<ModelBaseResult<ItemResponse>> GetItem(Guid shoppingCartUid, Guid itemUid)
        {
            return await _proxyHttpClient.GetAsync<ItemResponse>(ItemUriWithUid(shoppingCartUid, itemUid));
        }
        
        public async Task<BaseResult> DeleteItem(Guid shoppingCartUid, Guid itemUid)
        {
            return await _proxyHttpClient.DeleteAsync(ItemUriWithUid(shoppingCartUid, itemUid));
        }
    }
}