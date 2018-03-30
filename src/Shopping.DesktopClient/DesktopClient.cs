using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Shopping.Proxy;
using Shopping.Proxy.Infrastructure.Results;

namespace Shopping.DesktopClient
{
    [UsedImplicitly]
    public class DesktopClient
    {
        private readonly IShoppingCartProxy _shoppingCartProxy;
        private readonly IItemProxy _itemProxy;

        public DesktopClient(IShoppingCartProxy shoppingCartProxy, IItemProxy itemProxy)
        {
            _shoppingCartProxy = shoppingCartProxy;
            _itemProxy = itemProxy;
        }

        public async Task RunTest()
        {
            var createShoppingCartResult = await _shoppingCartProxy.CreateShoppingCart();
            await CreateItem(createShoppingCartResult.Result.Uid, "First item", 1);
            await CreateItem(createShoppingCartResult.Result.Uid, "Second item", 2);
            await CreateItem(createShoppingCartResult.Result.Uid, "Another item", 3);
            await ListShoppingCarts();
            PrintSeperator();
            await DeleteItem();
            PrintSeperator();
            await ListShoppingCarts();
            PrintSeperator();
            await EmptyShoppingCart();
            PrintSeperator();
            await ListShoppingCarts();
            PrintSeperator();
            await DeleteEmptyShoppingCarts();
            PrintSeperator();
            await ListShoppingCarts();
        }

        private async Task DeleteEmptyShoppingCarts()
        {
            var result = await _shoppingCartProxy.ListShoppingCarts(true);
            foreach (var shoppingCart in result.Result.Where(sc => !sc.ItemList.Any()))
            {
                var deleteResult = await _shoppingCartProxy.DeleteShoppingCart(shoppingCart.Uid);
                Console.WriteLine(deleteResult.HttpStatusCode == HttpStatusCode.OK
                    ? $"Deleting shopping cart with uid: {shoppingCart.Uid}"
                    : $"Failed deleting. Status: {deleteResult.HttpStatusCode}. Response: {deleteResult.Message}");
            }
        }

        private void PrintSeperator()
        {
             Console.WriteLine("----------------");   
        }
        
        private async Task DeleteItem()
        {
            var result = await _shoppingCartProxy.ListShoppingCarts(true);
            var firstCartWithItems = result.Result.FirstOrDefault(sc => sc.ItemList.Any());
            if (firstCartWithItems == null)
                return;
            var itemToDelete = firstCartWithItems.ItemList.First();
            var deleteResult = await _itemProxy.DeleteItem(firstCartWithItems.Uid, itemToDelete.Uid);
            Console.WriteLine(deleteResult.HttpStatusCode == HttpStatusCode.OK
                ? $"Item deleted from shopping cart {firstCartWithItems.Uid} with uid: {itemToDelete.Uid}"
                : $"Failed deleting. Status: {deleteResult.HttpStatusCode}. Response: {deleteResult.Message}");
        }

        private async Task EmptyShoppingCart()
        {
            var result = await _shoppingCartProxy.ListShoppingCarts(true);
            var firstCartWithItems = result.Result.FirstOrDefault(sc => sc.ItemList.Any());
            if (firstCartWithItems == null)
                return;

            var emptyResult = await _shoppingCartProxy.EmptyShoppingCart(firstCartWithItems.Uid);
            Console.WriteLine(emptyResult.HttpStatusCode == HttpStatusCode.OK
                ? $"Emptying shopping cart {firstCartWithItems.Uid}"
                : $"Failed emptying. Status: {emptyResult.HttpStatusCode}. Response: {emptyResult.Message}");
        }

        private async Task CreateItem(Guid shoppingCartUid, string description, int quantity)
        {
            await _itemProxy.CreateItem(shoppingCartUid, description, quantity);
        }

        private async Task ListShoppingCarts()
        {
            var result = await _shoppingCartProxy.ListShoppingCarts(true);
            result.Result.ForEach(shoppingCart =>
            {
                Console.WriteLine($"Shopping cart with uid: {shoppingCart.Uid} | " +
                                  $"Number of items in cart: {shoppingCart.ItemList.Count}");
            });
        }
    }
}