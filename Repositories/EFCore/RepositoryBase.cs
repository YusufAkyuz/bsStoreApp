using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;

namespace Repositories.EFCore;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    
    //Veriler Üzerinde İşlem Yapmak için Bir Context'e ihitiyacımız var bu context daha sonra book repositorydekiyle
    //  aynı context olması adına burda da seni kullanacak bir sınıf oluşacak ve ondada bir context olacak
    //  o sınıfın kullandığu context'i alıp öyle CRUD işlemlerini gerçekleştir diyoruz
    
    
    protected readonly RepositoryContext _context;
    
    public RepositoryBase(RepositoryContext context)
    {
        _context = context;
    }

    public IQueryable<T> FindAll(bool trackChanges) =>
        trackChanges ? _context.Set<T>().AsNoTracking() : _context.Set<T>();

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
        !trackChanges ? _context.Set<T>().Where(expression).AsNoTracking() : _context.Set<T>().Where(expression);

    public void Create(T entity) => _context.Set<T>().Add(entity);

    public void Update(T entity) => _context.Set<T>().Update(entity);

    public void Delete(T entity) => _context.Set<T>().Remove(entity);
}