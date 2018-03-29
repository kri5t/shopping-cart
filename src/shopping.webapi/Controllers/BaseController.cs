using System;
using Microsoft.AspNetCore.Mvc;
using Shopping.Core.Infrastructure.Mediation;

namespace Shopping.Webapi.Controllers
{
    public class BaseController : Controller
    {
        protected IActionResult MapToResult<TResponse>(TResponse response, Func<TResponse, IActionResult> result) 
            where TResponse : BaseResponse
        {
            return !response.HasError ? result(response) 
                                      : Error(response);
        }

        private IActionResult Error(BaseResponse response)
        {
            return ConvertToHttpResponse(response.ErrorCode, 
                string.Format("Request failed with error {0} and code {1}", response.ErrorMessage, response.ErrorCode));
        }
        
        private IActionResult ConvertToHttpResponse(ErrorCode errorCode, string message)
        {
            switch (errorCode)
            {
                case ErrorCode.NotValid:
                    return BadRequest(message);
                case ErrorCode.NotFound:
                    return NotFound(message);
                default:
                    throw new Exception(string.Format("Invalid ErrorCode '{0}', cannot return error", errorCode));
            }
        }
    }
}