using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Kros.Services
{
    public interface IBaseRepositoryAsync<T> where T : BaseEntity
    {
        IQueryable<T> GetAsQueryable(List<Expression<Func<T, bool>>> predicates = null, params Expression<Func<T, object>>[] includes);  
        Task<IEnumerable<T>> GetAsEnumerable(List<Expression<Func<T, bool>>> predicates = null, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetWhereAsEnumerable(IQueryable<T> queryable = null);
        Task<T> GetById(Int32 id);
        Task<T> AddOrUpdate(T entity);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Remove(T entity);
        Task<int> SaveChanges();
        Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate = null);
        Task<int> CountAll();
        Task<int> CountWhere(Expression<Func<T, bool>> predicate = null);
       // Task<int> ExecuteSqlCommand(string sql, params object[] parameters);
       // Task<int> ExecuteSqlCommandInterpolated(FormattableString sql);
       // Task<IEnumerable<T>> GetFromSqlAsEnumerable(string sql, params object[] parameters);
       // Task<IEnumerable<T>> GetFromSqlInterpolatedAsEnumerable(FormattableString sql);
    }
}
