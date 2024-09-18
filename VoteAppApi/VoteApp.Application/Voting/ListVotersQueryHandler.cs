using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoteApp.Domain;
using VoteApp.Domain.Voting;

namespace VoteApp.Application.Voting
{
    public class ListVotersQueryHandler : IRequestHandler<ListVotersQuery, ICollection<ListVotersItem>>
    {
        private readonly IBaseRepository<Person> _personBaseRepo;
        private readonly IBaseRepository<Vote> _voteBaseRepo;

        public ListVotersQueryHandler(IBaseRepository<Person> personBaseRepo, IBaseRepository<Vote> voteBaseRepo)
        {
            _personBaseRepo = personBaseRepo;
            _voteBaseRepo = voteBaseRepo;
        }

        public async Task<ICollection<ListVotersItem>> Handle(ListVotersQuery query, CancellationToken cancellationToken)
        {
            var personsWithVoteStatus = await (
                                                    from person in _personBaseRepo.QueryableSet()
                                                    join vote in _voteBaseRepo.QueryableSet() on person.Id equals vote.VoterId into leftJoinedVotes
                                                    from leftJoinedVote in leftJoinedVotes.DefaultIfEmpty()
                                                    select new ListVotersItem
                                                    {
                                                        Id = person.Id,
                                                        Name = person.Name,
                                                        HasVoted = leftJoinedVote != null
                                                    }
                                                ).ToListAsync();

            return personsWithVoteStatus.OrderBy(x => x.Name).ToList();
        }
    }

    public class ListVotersQuery : IRequest<ICollection<ListVotersItem>>
    {
    }

    public class ListVotersItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool HasVoted { get; set; }
    }
}
