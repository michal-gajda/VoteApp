using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using VoteApp.Domain;
using VoteApp.Domain.Exceptions;
using VoteApp.Domain.Voting;

namespace VoteApp.Application.Voting
{
    public class AddVoterCommandHandler : IRequestHandler<AddVoterCommand, AddVoterResponse>
    {
        private readonly IBaseRepository<Person> _personBaseRepo;

        public AddVoterCommandHandler(IBaseRepository<Person> personBaseRepo)
        {
            _personBaseRepo = personBaseRepo;
        }

        public async Task<AddVoterResponse> Handle(AddVoterCommand request, CancellationToken cancellationToken)
        {

            var personFromDb = await _personBaseRepo.QueryableReadonly().FirstOrDefaultAsync(x => x.Name == request.Name);
            if (personFromDb != null)
            {
                throw new UserException($"This name is already used.", VoteAppErrorCodes.DataNotValid);
            }

            var voter = new Person(request.Name);

            await _personBaseRepo.AddAsync(voter);

            return new AddVoterResponse { Id = voter.Id };
        }
    }

    public class AddVoterCommand : IRequest<AddVoterResponse>
    {
        public string Name { get; set; } = null!;
    }

    public class AddVoterResponse
    {
        public Guid Id { get; set; }
    }

    public class AddVoterCommandValidator : AbstractValidator<AddVoterCommand>
    {
        public AddVoterCommandValidator()
        {
            RuleFor(x => x.Name).Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage("Name is required");
        }
    }
}
