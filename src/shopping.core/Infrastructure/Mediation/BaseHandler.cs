using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Shopping.Core.Infrastructure.Mediation
{
    public abstract class BaseHandler<TRequest, TResult> : IRequestHandler<TRequest, TResult> where TRequest : IRequest<TResult> where TResult : BaseResponse
    {
        public abstract Task<TResult> Handle(TRequest request, CancellationToken cancellationToken);
        
        protected TResult Error(ErrorCode errorCode, string errorMessage = null)
        {
            TResult instance = Activator.CreateInstance<TResult>();
            instance.ErrorCode = errorCode;
            instance.ErrorMessage = errorMessage;
            return instance;
        }
    }
}