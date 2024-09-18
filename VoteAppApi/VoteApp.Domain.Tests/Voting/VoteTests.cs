using NUnit.Framework;
using Shouldly;
using VoteApp.Domain.Voting;
using VoteApp.SharedCommon;

namespace VoteApp.Domain.Tests.Voting
{
    [TestFixture]
    public class VoteTests
    {
        public VoteTests()
        {
            /*
             * This is trival case of usage DateTimeProvider and not the best one, but there are business cases
             * when given object must fulfill rule being depended on specific date and in that cases specific current date needs to be set.
             */
            DateTimeProvider.SetDateTime(new DateTime(2004, 03, 03, 12, 10, 15, DateTimeKind.Utc));
        }

        [Test]
        public void ShouldSetCreateVoteSuccessfully()
        {
            // given
            var voterId = Guid.NewGuid();
            var candidateId = Guid.NewGuid();

            // when
            var newVote = new Vote(voterId, candidateId);

            // then
            newVote.VoterId.ShouldBe(voterId);
            newVote.CandidateId.ShouldBe(candidateId);
            newVote.CreateDate.ShouldBe(new DateTime(2004, 03, 03, 12, 10, 15, DateTimeKind.Utc));
        }
    }
}
