using Microsoft.EntityFrameworkCore;
using DAL.Entities;
using System.Linq.Expressions;

namespace DAL.Storage.Database;

public class DatabaseRepository<T> : IRepository<T> where T : BaseEntity
{
  private readonly FamilyTreeDbContext _context;

  public DatabaseRepository(FamilyTreeDbContext context)
  {
    _context = context;
  }

  public async Task<List<T>> GetAllAsync()
  {
    return await _context.Set<T>().ToListAsync();
  }

  public async Task AddAsync(T entity)
  {
    await _context.Set<T>().AddAsync(entity);
    await _context.SaveChangesAsync();
  }

  public async Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate)
  {
    return await _context.Set<T>()
      .Where(predicate)
      .ToListAsync();
  }

  public async Task AddAllAsync(List<T> products)
  {
    _context.Set<T>().UpdateRange(products);
    await _context.SaveChangesAsync();
  }

}
