using GuardNet;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using VoteApp.Api.Helpers;
using VoteApp.Application;
using VoteApp.Domain;
using VoteApp.Domain.Exceptions;
using VoteApp.Dto;

namespace VoteApp.Api.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    [Route("api")]
    public class ErrorController : ControllerBase
    {
        private readonly ILogger<ErrorController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly ICurrentUser _currentUser;

        public ErrorController(ILogger<ErrorController> logger, IWebHostEnvironment env, ICurrentUser currentUser)
        {
            _logger = logger;
            _env = env;
            _currentUser = currentUser;
        }

        [Route("error")]
        public ErrorResultDto? Error()
        {
            var errorContext = HttpContext.Features.Get<IExceptionHandlerFeature>();

            Guard.For<UnexpectedStateException>(() => errorContext == null, $"Incorrect  usage");

            var exception = errorContext!.Error;

            if (exception is TaskCanceledException)
            {
                return null;
            }

            var loggedUser = _currentUser.UserId ?? "N/A";
            var handlerName = HttpContext.Items["HandlerName"]?.ToString() ?? "N/A";
            var handlerInputData = HttpContext.Items["InputData"]?.ToString() ?? "N/A";
            var executionContextId = HttpContext.TraceIdentifier;

            var logMessage = $"Execution Context Id: {executionContextId} | Logged User: {loggedUser}, | Handler Name: {handlerName} | Handler Input Data: {handlerInputData}";
            _logger.LogError(exception, logMessage);

            SetStatusCode(exception);

            var errorDto = ErrorResultDtoFactory.Create(exception, executionContextId);

            if (_env.IsNotRestricted())
            {
                errorDto.AddTechDetails(new TechDetails { MethodName = handlerName, Exception = exception.ToString(), InnerException = exception.InnerException?.ToString() });
            }
            else
            {
                errorDto.ClearTechDetails();
            }

            return errorDto;
        }

        private void SetStatusCode(Exception exception)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;

            if (exception is UserException userExeption)
            {
                switch (userExeption.ErrorCode)
                {
                    case VoteAppErrorCodes.NotAuthorized:
                    case VoteAppErrorCodes.PermissionDenied:
                        Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        break;
                    default:
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                }
            }
        }
    }
}
