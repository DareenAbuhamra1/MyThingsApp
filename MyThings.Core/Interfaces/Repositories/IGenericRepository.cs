using System.Linq.Expressions;

namespace MyThings.Core.Interfaces;

// CRUD
public interface IGenericRepository<T> where T :class
{
    Task<T?> GetByIdAsync(int id);
    Task<T?> GetByPhoneAsync(string phone);
    Task<T> AddAsync(T entity); // Adds to the database
    Task AddRangeAsync(IEnumerable<T> entities);
    void Update(T entity); // Updates the database
    void Delete(T entity); // Delete from database set IsDeleted == true
    Task<bool> FindAsync(Expression<Func<T, bool>> predicate); // Find by condition
}