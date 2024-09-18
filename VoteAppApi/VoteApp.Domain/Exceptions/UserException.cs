using GuardNet;
using System;
using System.Collections.Generic;

namespace VoteApp.Domain.Exceptions
{
    public class UserException : Exception
    {
        private readonly List<string> _messages = new();

        public UserException(string message, VoteAppErrorCodes errorCode = VoteAppErrorCodes.DataNotValid)
        {
            Guard.NotNullOrWhitespace(message, nameof(message));

            _messages.Add(message);
            ErrorCode = errorCode;
        }

        public UserException(List<string> messages, VoteAppErrorCodes errorCode = VoteAppErrorCodes.DataNotValid)
        {
            Guard.NotNull(messages, nameof(messages));
            Guard.For<ArgumentException>(() => messages.Count == 0 || messages.Exists(x => string.IsNullOrWhiteSpace(x)), "Messages cannot be empty");

            _messages.AddRange(messages);
            ErrorCode = errorCode;
        }

        public VoteAppErrorCodes ErrorCode { get; private set; }

        public void AddMessage(string exceptionMessage)
        {
            Guard.NotNullOrWhitespace(exceptionMessage, nameof(exceptionMessage));
            _messages.Add(exceptionMessage);
        }

        public List<string> Messages => _messages;

        public new string Message => throw new UnexpectedStateException($"Use Messages");
    }
}
