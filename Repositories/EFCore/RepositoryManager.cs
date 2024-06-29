using Repositories.Contracts;

namespace Repositories.EFCore;

public class RepositoryManager : IRepositoryManager
{

    private readonly RepositoryContext _context;
    
    //Burda Lazyloading yaptık yani nesneye ihtiyaç duyulduğunda ancak create edilsin dedik
    
    private readonly Lazy<IBookRepository> _bookRepository;

    public RepositoryManager(RepositoryContext context)
    {
        _context = context;
        _bookRepository = new Lazy<IBookRepository>(() => new BookRepository(_context));
    }

    public IBookRepository Book => _bookRepository.Value;
    
    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}