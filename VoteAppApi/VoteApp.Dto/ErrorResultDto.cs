using System.Collections.Generic;
using System.Linq;
using VoteApp.Domain;
using VoteApp.Domain.Exceptions;

namespace VoteApp.Dto
{
    public class ErrorResultDto
    {
        public ErrorResultDto(string message, string errorLogId)
        {
            Messages.Add(message);
            ErrorLogId = errorLogId;
        }

        public ErrorResultDto(UserException userException, string errorLogId)
        {
            ErrorLogId = errorLogId;
            Messages.AddRange(userException.Messages.Where(x => !string.IsNullOrWhiteSpace(x)));
        }

        public List<string> Messages { get; set; } = new();
        public VoteAppErrorCodes ErrorCode { get; private set; } = VoteAppErrorCodes.NotCategorized;

        public string ErrorCodeName => ErrorCode.ToString();
        public string ErrorLogId { get; private set; }

        public TechDetails? TechDetails { get; private set; }
        public void AddTechDetails(TechDetails techDetails)
        {
            TechDetails = techDetails;
        }
        public void ClearTechDetails()
        {
            TechDetails = null;
        }
    }

    public class TechDetails
    {
        public string MethodName { get; set; } = string.Empty;
        public string Exception { get; set; } = string.Empty;
        public string? InnerException { get; set; }
    }
}
