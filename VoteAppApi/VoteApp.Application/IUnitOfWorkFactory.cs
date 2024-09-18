namespace VoteApp.Application
{
    /*
     * [DESC]
     * Sometimes, we need to create an additional database context. One such situation is when creating a new flow for a database transaction.
     */
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork CreateUnitOfWork();
    }
}
