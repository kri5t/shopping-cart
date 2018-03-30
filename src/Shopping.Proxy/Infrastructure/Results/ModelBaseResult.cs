namespace Shopping.Proxy.Infrastructure.Results
{
    public class ModelBaseResult<TModel> : BaseResult
    {
        public TModel Result { get; set; }
    }
}