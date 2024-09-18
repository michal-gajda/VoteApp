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
    public class ListCandidatesQueryHandler : IRequestHandler<ListCandidatesQuery, ICollection<ListCandidatesItem>>
    {
        private readonly IBaseRepository<Person> _personBaseRepo;
        private readonly IBaseRepository<Vote> _voteBaseRepo;

        public ListCandidatesQueryHandler(IBaseRepository<Person> personBaseRepo, IBaseRepository<Vote> voteBaseRepo)
        {
            _personBaseRepo = personBaseRepo;
            _voteBaseRepo = voteBaseRepo;
        }

        public async Task<ICollection<ListCandidatesItem>> Handle(ListCandidatesQuery query, CancellationToken cancellationToken)
        {
            var candidatesWithVotes = await (
                                                from person in _personBaseRepo.QueryableSet()
                                                where person.IsCandidate
                                                join vote in _voteBaseRepo.QueryableSet() on person.Id equals vote.CandidateId into leftJoinedVotes
                                                from leftJoinedVote in leftJoinedVotes.DefaultIfEmpty()
                                                group leftJoinedVote by new { person.Id, person.Name } into grouped
                                                select new ListCandidatesItem
                                                {
                                                    Id = grouped.Key.Id,
                                                    Name = grouped.Key.Name,
                                                    Votes = grouped.Count(v => v != null)
                                                }
                                            ).ToListAsync();


            return candidatesWithVotes.OrderBy(x => x.Name).ToList();
        }
    }

    public class ListCandidatesQuery : IRequest<ICollection<ListCandidatesItem>>
    {
    }

    public class ListCandidatesItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int Votes { get; set; }
    }
}
