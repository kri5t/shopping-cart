using System;
using System.Threading.Tasks;
using Shopping.Proxy;
using Shopping.Proxy.Infrastructure.Results;

namespace Shopping.DesktopClient
{
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
            await _itemProxy.CreateItem(createShoppingCartResult.Result.Uid, "something", 23);
            await _itemProxy.CreateItem(createShoppingCartResult.Result.Uid, "something", 23);
            await _itemProxy.CreateItem(createShoppingCartResult.Result.Uid, "something", 23);
            var result = await _shoppingCartProxy.ListShoppingCarts();
            PrintResult(result);
            result.Result.ForEach(shoppingCart =>
            {
                Console.WriteLine(shoppingCart.Uid);    
            });
        }
        
        private void PrintResult(BaseResult result)
        {
            Console.WriteLine("WRITING RESULTS MY FRIEND!");
            Console.WriteLine(result.HttpStatusCode);
            Console.WriteLine(result.Message);
        }
    }
}