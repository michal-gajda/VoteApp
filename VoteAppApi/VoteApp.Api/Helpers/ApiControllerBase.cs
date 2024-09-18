using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VoteApp.Application;

namespace VoteApp.Api.Helpers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected readonly ICurrentUser _currentUser;
        protected readonly IMediator _mediator;

        protected ApiControllerBase(ICurrentUser currentUser, IMediator mediator)
        {
            _currentUser = currentUser;
            _mediator = mediator;
        }
    }
}
