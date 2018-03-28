namespace Shopping.Core.Infrastructure.Mediation
{
    public class BaseResponse
    {
        public ErrorCode ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public bool HasError => ErrorCode != ErrorCode.NoError;
    }
}