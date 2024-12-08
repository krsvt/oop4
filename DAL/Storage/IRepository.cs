using DAL.Entities;
using System.Linq.Expressions;

namespace DAL.Storage;

public interface IRepository<T> where T : BaseEntity
{
  Task<List<T>> GetAllAsync();
  Task<T> GetAsync(int id);
  Task AddAsync(T entity);
  Task<List<T>> GetByIdsAsync(List<int> ids);
  Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate);
  Task AddAllAsync(List<T> products);
}
