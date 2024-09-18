using FluentValidation;
using GuardNet;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoteApp.Domain;
using VoteApp.Domain.Voting;

namespace VoteApp.Application.Voting
{
    public class VoteCommandTHandler : IRequestHandler<VoteCommand, Unit>
    {
        private readonly IBaseRepository<Person> _personBaseRepo;
        private readonly IBaseRepository<Vote> _voteBaseRepo;

        public VoteCommandTHandler(IBaseRepository<Person> personBaseRepo, IBaseRepository<Vote> voteBaseRepo)
        {
            _personBaseRepo = personBaseRepo;
            _voteBaseRepo = voteBaseRepo;
        }

        public async Task<Unit> Handle(VoteCommand request, CancellationToken cancellationToken)
        {
            var voter = await _personBaseRepo.GetAsync(request.VoterId);
            var candidate = await _personBaseRepo.GetAsync(request.CandidateId);

            Guard.For<UnexpectedStateException>(() => !candidate.IsCandidate, $"Invalid candidate.");

            var vote = await _voteBaseRepo.QueryableReadonly().Where(x => x.VoterId == voter.Id).SingleOrDefaultAsync();
            Guard.For<UnexpectedStateException>(() => vote != null, $"This voter has already voted.");

            // Utworzenie nowego głosu
            vote = new Vote(voter.Id, candidate.Id);
            await _voteBaseRepo.AddAsync(vote);

            return Unit.Value;
        }
    }

    public class VoteCommand : IRequest<Unit>
    {
        public Guid VoterId { get; set; }
        public Guid CandidateId { get; set; }
    }

    public class VoteCommandValidator : AbstractValidator<VoteCommand>
    {
        public VoteCommandValidator()
        {
            RuleFor(x => x.VoterId).Must(x => x != Guid.Empty)
                .WithMessage("VoterId is required");
            RuleFor(x => x.CandidateId).Must(x => x != Guid.Empty)
                .WithMessage("CandidateId is required");
        }
    }
}
