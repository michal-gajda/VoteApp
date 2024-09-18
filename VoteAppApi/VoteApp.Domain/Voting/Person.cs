using System;

namespace VoteApp.Domain.Voting
{
    public class Person
    {
        public Guid Id { get; private set; }

        private string _name = string.Empty;
        public string Name
        {
            get
            {
                return _name;
            }

            private set => _name = value.Trim().ToLowerInvariant();
        }

        public bool IsCandidate { get; private set; }

        private Person() { }

        public Person(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public void SetAsCandidate()
        {
            IsCandidate = true;
        }
    }

    public static class PersonExtensions
    {
        public static Person BuildCandidate(this Person person)
        {
            person.SetAsCandidate();
            return person;
        }
    }

}
