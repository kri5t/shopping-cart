using System.Net;

namespace Shopping.Proxy.Infrastructure.Results
{
    public class BaseResult
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public string Message { get; set; }
    }
}