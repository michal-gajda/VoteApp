using System;

namespace VoteApp.Domain.Voting
{
    public class Vote
    {
        public Guid VoterId { get; private set; }
        public Guid CandidateId { get; private set; }
        public DateTime CreateDate { get; private set; }

        private Vote() { }

        public Vote(Guid voterId, Guid candidateId)
        {
            VoterId = voterId;
            CandidateId = candidateId;
            CreateDate = DateTimeProvider.Now;
        }
    }
}
