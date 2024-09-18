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
    /*
     * [DESC]
     * According to the ubiquitous language approach (DDD), we avoid technical terms as much as possible at every stage of software engineering, including the codebase.
     * Thus, we do not create folders or files with names like Commands, Queries, Interfaces, DTOs, etc., as these names sound too technical. Instead, we use business language.
     * When naming handlers, we try to describe the functionality they implement(use case). Of course, it is unlikely that we will achieve 100% business language in the codebase.
     */
    public class AddCandidateCommandHandler : IRequestHandler<AddCandidateCommand, AddCandidateResponse>
    {
        private readonly IBaseRepository<Person> _personBaseRepo;

        public AddCandidateCommandHandler(IBaseRepository<Person> personBaseRepo)
        {
            _personBaseRepo = personBaseRepo;
        }

        public async Task<AddCandidateResponse> Handle(AddCandidateCommand request, CancellationToken cancellationToken)
        {
            var personFromDb = await _personBaseRepo.QueryableReadonly().FirstOrDefaultAsync(x => x.Name == request.Name.ToLowerInvariant());
            if (personFromDb != null)
            {
                throw new UserException($"This name is already used.", VoteAppErrorCodes.DataNotValid);
            }

            var candidate = new Person(request.Name);
            candidate.SetAsCandidate();

            await _personBaseRepo.AddAsync(candidate);

            return new AddCandidateResponse { Id = candidate.Id };
        }
    }

    /*
     * [DESC]
     * There is no need to create separate files for the following classes since they are unlikely to be modified by two programmers at the same time.
     * Keeping them all here helps avoid creating a large number of files in the project.
     */
    public class AddCandidateCommand : IRequest<AddCandidateResponse>
    {
        public string Name { get; set; } = null!;
    }

    public class AddCandidateResponse
    {
        public Guid Id { get; set; }
    }

    public class AddCandidateCommandValidator : AbstractValidator<AddCandidateCommand>
    {
        public AddCandidateCommandValidator()
        {
            RuleFor(x => x.Name).Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage("Name is required");
        }
    }
}
