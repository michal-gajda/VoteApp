using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using VoteApp.Api.Helpers;
using VoteApp.Application;
using VoteApp.Application.Voting;

namespace VoteApp.Api.Controllers
{
    public class VotingController : ApiControllerBase
    {
        public VotingController(ICurrentUser currentUser, IMediator mediator) : base(currentUser, mediator)
        {
        }

        [HttpPost("add-voter")]
        public async Task<AddVoterResponse> AddVoter(AddVoterCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPost("add-candidate")]
        public async Task<AddCandidateResponse> AddCandidate(AddCandidateCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPost("vote")]
        public async Task<IActionResult> Vote(VoteCommand command)
        {
            await _mediator.Send(command);

            return NoContent();
        }

        [HttpGet("voters")]
        public async Task<ICollection<ListVotersItem>> ListVoters()
        {
            return await _mediator.Send(new ListVotersQuery());
        }

        [HttpGet("candidates")]
        public async Task<ICollection<ListCandidatesItem>> ListCandidates()
        {
            return await _mediator.Send(new ListCandidatesQuery());
        }
    }
}
