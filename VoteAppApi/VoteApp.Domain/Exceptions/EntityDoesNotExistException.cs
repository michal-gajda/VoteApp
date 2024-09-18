using System;

namespace VoteApp.Domain.Exceptions
{
    public class EntityDoesNotExistException : Exception
    {
        public EntityDoesNotExistException(string entityName, string data) : base($"Entity {entityName} does not exist: {data}") { }
    }
}
