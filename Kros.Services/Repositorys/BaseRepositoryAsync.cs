using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Kros.Services
{
    public class BaseRepositoryAsync<T> : IBaseRepositoryAsync<T> where T : BaseEntity
    {
        protected ApplicationContext Context;

        public BaseRepositoryAsync(ApplicationContext context) => Context = context ?? throw new NullReferenceException("context not initialized");

        public async Task<int> CountAll() => await Context.Set<T>().AsNoTracking().CountAsync();

        public async Task<int> CountWhere(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate != null)
                return await Context.Set<T>().AsNoTracking().CountAsync(predicate);
            else
                return await Context.Set<T>().AsNoTracking().CountAsync();
        }

        public async Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate != null)
                return await Context.Set<T>().FirstOrDefaultAsync(predicate);
            else
                return await Context.Set<T>().FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAsEnumerable(List<Expression<Func<T, bool>>> predicates = null, params Expression<Func<T, object>>[] includes)
            => await GetAsQueryable(predicates, includes).ToListAsync();
        /*public async Task<IEnumerable<T>> GetAllAsEnumerable() {
            throw new Exception("gdfgfgdg");
            return await GetAllAsQueryable().ToListAsync();
        }*/

        public IQueryable<T> GetAsQueryable(List<Expression<Func<T, bool>>> predicates = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = Context.Set<T>().AsQueryable<T>();
            foreach (Expression<Func<T, object>> include in includes)
            {
                MemberExpression memberExpression = include.Body as MemberExpression;
                if (memberExpression != null)
                    query = query.Include(memberExpression.Member.Name);            
            }

            Expression<Func<T, bool>> expresCombined = null;            
            if (predicates?.Count > 0)
            {
                expresCombined = PredicateBuilder.True<T>(); //True для And / False для Or
                foreach (Expression<Func<T, bool>> item in predicates)
                {
                    expresCombined = expresCombined.And(item);
                }
            }

            return expresCombined != null ? query.Where(expresCombined).AsNoTracking() : query.AsNoTracking();
        }
            //=> Context.Set<T>().AsNoTracking();

        public async Task<T> GetById(int id) => await Context.Set<T>().FindAsync(id);

        public async Task<IEnumerable<T>> GetWhereAsEnumerable(IQueryable<T> queryable = null)
        {
            IQueryable<T> query = Context.Set<T>().AsQueryable<T>();
            if (queryable != null) query = queryable;
            return await query.AsNoTracking().ToListAsync<T>();
        }

        public async Task<T> Remove(T entity)
        {
            if (entity == null) throw new NullReferenceException("entity not initialized");
            //var m = Context.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified);
            T entityTracked = Context.ChangeTracker.Entries<T>().Select((EntityEntry e) => (T)e.Entity).SingleOrDefault(e => e.Id.Equals(entity.Id));
            T copy;
            if (entityTracked != null)// If entity is tracked just delete it
            {
                copy = entityTracked;
                Context.Remove(entityTracked);
            }
            else// If not tracked, just attche entity to context and remove.                                                                                                   
            {
                copy = entity;
                Context.Set<T>().Remove(entity);
            }
            if (await Context.SaveChangesAsync() > 0)
                return copy;
            else
                return null;
        }

        public async Task<T> Update(T entity)
        {
            if (entity == null) throw new NullReferenceException("entity not initialized");
            // Context.Set<T>().Add(entity);
            //  Context.Entry(entity).CurrentValues.SetValues(entity);
            Context.Entry(entity).State = EntityState.Modified;
            if (await Context.SaveChangesAsync() > 0)
                return entity;
            else
                return null;
        }

        public async Task<T> Add(T entity)
        {
            if (entity == null) throw new NullReferenceException("entity not initialized");
            // await Context.AddAsync(entity);
            await Context.Set<T>().AddAsync(entity);
            if (await Context.SaveChangesAsync() > 0)
                return entity;
            else
                return null;
        }

        public async Task<T> AddOrUpdate(T entity)
        {
            if (entity == null) throw new NullReferenceException("entity not initialized");
            T ph = GetById(entity.Id).Result;
            if (ph != null)
            {
                Context.Entry(ph).State = EntityState.Detached;
                Context.Attach(entity).State = EntityState.Modified;
            }
            else
            {
                await Context.Set<T>().AddAsync(entity);
            }
            if (await Context.SaveChangesAsync() > 0)
                return entity;
            else
                return null;
        }

       /* public async Task<int> ExecuteSqlCommand(string sql, params object[] parameters) 
        {
            if (String.IsNullOrEmpty(sql)) throw new ArgumentNullException("sql command is null");
            if (parameters == null) throw new ArgumentNullException("parameters command not initialized");
            return await Context.Database.ExecuteSqlRawAsync(sql, parameters);
        }*/

       /* public async Task<IEnumerable<T>> GetFromSqlAsEnumerable(string sql, params object[] parameters)
        {
            if (String.IsNullOrEmpty(sql)) throw new ArgumentNullException("sql command is null");
            if (parameters == null) throw new ArgumentNullException("parameters command not initialized");
            return await Context.Set<T>().FromSqlRaw(sql, parameters).AsNoTracking().ToListAsync<T>();
        }*/

        public async Task<int> SaveChanges()  => await Context.SaveChangesAsync();

       /* public async Task<int> ExecuteSqlCommandInterpolated(FormattableString sql)
        {
            if (String.IsNullOrEmpty(sql.ToString())) throw new ArgumentNullException("sql command is null");
            return await Context.Database.ExecuteSqlInterpolatedAsync(sql);
        }*/

       /* public async Task<IEnumerable<T>> GetFromSqlInterpolatedAsEnumerable(FormattableString sql)
        {
            if (String.IsNullOrEmpty(sql.ToString())) throw new ArgumentNullException("sql command is null");
            return await Context.Set<T>().FromSqlInterpolated(sql).AsNoTracking().ToListAsync<T>();
        }*/
    }
}
