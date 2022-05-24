using Domain.Results;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITTPTestProject.Controllers
{
    public class BaseController : ControllerBase
    {
        internal ActionResult<ResponseResult<TResult>> GetResponse<TResult>(Result<TResult> result)
        {
            var responceResult = new ResponseResult<TResult>
            {
                Message = result.Message,
                Status = (int)result.Error,
                IsSuccess = result.IsSucceess,
                Value = result.Value,
                TraceId = HttpContext.TraceIdentifier
            };

            responceResult.Title = Constants.ResponseTitleSuceess;

            responceResult.Type = GetResponseType(responceResult.Status);

            return Ok(responceResult);

        }
        internal ActionResult<ResponseResult<TResult>> GetErrorResponse<TResult>(Result<TResult> result)
        {
            var responceResult = new ResponseResult<TResult>
            {
                Message = result.Message,
                Status = (int)result.Error,
                IsSuccess = result.IsSucceess,
                TraceId = HttpContext.TraceIdentifier
            };

            responceResult.Title = Constants.ResponseTitleFailed;

            responceResult.Type = GetResponseType(responceResult.Status);

            return result.Error switch
            {
                Error.NotFound => NotFound(responceResult),
                Error.Conflict => Conflict(responceResult),
                Error.Unauthorized => Unauthorized(responceResult),
                Error.BadRequest => BadRequest(responceResult),
                Error.Forbidden => StatusCode(403, responceResult),
                _ => StatusCode(500)
            };
        }
        internal ActionResult<ResponseResult> GetResponse(Result result)
        {
            var responceResult = new ResponseResult
            {
                Message = result.Message,
                Status = (int)result.Error,
                IsSuccess = result.IsSucceess,
                TraceId = HttpContext.TraceIdentifier
            };

            responceResult.Title = Constants.ResponseTitleSuceess;

            responceResult.Type = GetResponseType(responceResult.Status);

            return Ok(responceResult);

        }
        internal ActionResult<ResponseResult> GetErrorResponse(Result result)
        {
            var responceResult = new ResponseResult
            {
                Message = result.Message,
                Status = (int)result.Error,
                IsSuccess = result.IsSucceess,
                TraceId = HttpContext.TraceIdentifier
            };

            responceResult.Title = Constants.ResponseTitleFailed;

            responceResult.Type = GetResponseType(responceResult.Status);

            return result.Error switch
            {
                Error.NotFound => NotFound(responceResult),
                Error.Conflict => Conflict(responceResult),
                Error.Unauthorized => Unauthorized(responceResult),
                Error.BadRequest => BadRequest(responceResult),
                Error.Forbidden => StatusCode(403, responceResult),
                _ => StatusCode(500)
            };
        }
        internal ActionResult<ResponseResult<TResult>> GetErrorResponse<TResult>(Result result)
        {
            var responceResult = new ResponseResult<TResult>
            {
                Message = result.Message,
                Status = (int)result.Error,
                IsSuccess = result.IsSucceess,
                TraceId = HttpContext.TraceIdentifier
            };

            responceResult.Title = Constants.ResponseTitleFailed;

            responceResult.Type = GetResponseType(responceResult.Status);

            return result.Error switch
            {
                Error.NotFound => NotFound(responceResult),
                Error.Conflict => Conflict(responceResult),
                Error.Unauthorized => Unauthorized(responceResult),
                Error.BadRequest => BadRequest(responceResult),
                Error.Forbidden => StatusCode(403, responceResult),
                _ => StatusCode(500)
            };
        }
        private static Uri? GetResponseType(int statusCode)
        {
            return statusCode switch
            {
                200 => new Uri(Constants.OkResponseType),
                400 => new Uri(Constants.BadRequestResponseType),
                401 => new Uri(Constants.UnauthorizedResponseType),
                403 => new Uri(Constants.ForbiddenResponseType),
                404 => new Uri(Constants.NotFoundResponseType),
                409 => new Uri(Constants.ConflictResponseType),
                500 => new Uri(Constants.InternalServerErrorResponseType),
                _ => null,
            };
        }
    }
}

