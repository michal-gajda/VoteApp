using NUnit.Framework;
using Shouldly;
using VoteApp.Application.Voting;
using VoteApp.Domain.Exceptions;
using VoteApp.Domain.Voting;
using VoteApp.Tests.Common;

namespace VoteApp.Application.Tests.Voting
{
    [TestFixture]
    public class AddCandidateCommandHandlerTests : BaseTestContainer
    {
        [SetUp]
        public void SetupBeforeEachTest()
        {
            _dbContext.ClearDatabase();
        }

        [TestCase("First Name")]
        [TestCase("First name")]
        [TestCase("first Name")]
        [TestCase("first name")]
        [TestCase("FIRST NAME")]
        [TestCase("FIRSt NAMe")]
        public async Task ShouldAddCandidateSuccessfully(string newPersonName)
        {
            // given
            var expectedName = "first name";

            var input = new AddCandidateCommand { Name = newPersonName };

            // when
            var output = await _mediator.Send(input);

            // then            
            output.Id.ShouldNotBe(Guid.Empty);

            _dbContext.Persons.Count().ShouldBe(1);
            var candidateCreated = _dbContext.Persons.Single(p => p.Id == output.Id);
            candidateCreated.Name.ShouldBe(expectedName);
            candidateCreated.IsCandidate.ShouldBeTrue();
        }


        [TestCase("First Name")]
        [TestCase("First name")]
        [TestCase("first Name")]
        [TestCase("first name")]
        [TestCase("FIRST NAME")]
        [TestCase("FIRSt NAMe")]
        public async Task ShouldThrowWhenUsernameExists(string newPersonName)
        {
            // given
            _dbContext.Persons.Add(new Person("First Name"));
            _dbContext.SaveChanges();

            var input = new AddCandidateCommand { Name = newPersonName };
            // when-then
            (await Should.ThrowAsync<UserException>(async () => await _mediator.Send(input)))
                .Messages[0].ShouldContain("This name is already used.");

        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public async Task ShouldThrowWhenEmpty_RequiredValidation(string newPersonName)
        {
            // given
            var input = new AddCandidateCommand { Name = newPersonName };

            // when-then
            (await Should.ThrowAsync<UserException>(async () => await _mediator.Send(input)))
                .Messages[0].ShouldContain("Name is required");
        }
    }
}
