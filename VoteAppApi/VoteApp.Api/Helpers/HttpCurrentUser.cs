using GuardNet;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using VoteApp.Application;

namespace VoteApp.Api.Helpers
{
    public class HttpCurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpCurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? UserId
        {
            get
            {
                Guard.For<UnexpectedStateException>(() => _httpContextAccessor.HttpContext == null, $"Incorrect Usage");

                var nameFromClaim = _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Name)?.Value;

                return nameFromClaim;
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                Guard.For<UnexpectedStateException>(() => _httpContextAccessor.HttpContext == null, $"Incorrect Usage");

                var nameFromClaim = _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Name)?.Value;

                return !string.IsNullOrWhiteSpace(nameFromClaim);
            }
        }
    }
}
