namespace Repositories.Contracts;

public interface IRepositoryManager
{
    public IBookRepository Book { get; }
    Task SaveAsync();
}