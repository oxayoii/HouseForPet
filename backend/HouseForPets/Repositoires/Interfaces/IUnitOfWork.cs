namespace Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IPetRepository pets { get; }
        IUserRepository users { get; }
        IFavRepository favs { get; }
        Task BeginTransactionAsync();
        Task CommitAsync();
        void Dispose();
        Task RollbackAsync();
    }
}