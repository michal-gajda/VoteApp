using Microsoft.EntityFrameworkCore;
using Polly.CircuitBreaker;
using System;
using System.Data.Common;
using System.Linq;
using VoteApp.Api.StartupConfigs;
using VoteApp.Domain;
using VoteApp.Domain.Exceptions;
using VoteApp.Dto;

namespace VoteApp.Api.Helpers
{
    public static class ErrorResultDtoFactory
    {
        public static ErrorResultDto Create(Exception exception, string errorLogId)
        {
            if (exception is UserException ue)
            {
                return new ErrorResultDto(ue, errorLogId);
            }
            else if (exception is DbUpdateConcurrencyException)
            {
                return new ErrorResultDto(new UserException("The functionality has encountered temporary problem. Please try again.", VoteAppErrorCodes.Other), errorLogId);
            }
            else if ((exception is DbException && PollyMiddleware.DbExceptionCodes.Any(dbErrorCode => exception.Message.Contains(dbErrorCode)))
                        || exception is BrokenCircuitException)
            {
                return new ErrorResultDto(new UserException("The functionality is busy. Please try again in a while.", VoteAppErrorCodes.DbTimeoutError), errorLogId);
            }

            return new ErrorResultDto(new UserException($"The functionality has encountered problem. Please contact the support. Error Id: {errorLogId}", VoteAppErrorCodes.ServerError), errorLogId);
        }
    }
}
