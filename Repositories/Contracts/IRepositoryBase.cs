using System.Linq.Expressions;

namespace Repositories.Contracts;

public interface IRepositoryBase<T>
{
    //Burda CRUD yapısı üzerinde çalışıyoruz => Create Read Update Delete
    
    IQueryable<T> FindAll(bool trackChanges);
    IQueryable<T> FindByCondition(Expression<Func<T,bool>> expression, bool trackChanges);
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
}