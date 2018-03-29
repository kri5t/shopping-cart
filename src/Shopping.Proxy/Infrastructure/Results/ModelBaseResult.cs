using System.Net;

namespace Shopping.Proxy.Infrastructure.Results
{
    public class ModelBaseResult<TModel> : BaseResult
    {
        public TModel Model { get; set; }
    }
}