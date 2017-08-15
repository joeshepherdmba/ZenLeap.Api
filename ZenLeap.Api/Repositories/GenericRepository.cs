using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZenLeap.Api.Data;

namespace ZenLeap.Api.Repositories
{
    public class GenericRepository<T> : IDisposable, IGenericRepository<T> where T : class
    {
        protected DbSet<T> DbSet;
        protected DataContext context;

        public GenericRepository(DataContext dataContext)
        {
            context = dataContext;
            DbSet = context.Set<T>();
        }

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        public IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        public IEnumerable<T> GetAll()
        {
            return DbSet;
        }

        public T GetById(object id)
        {
            return DbSet.Find(id);
        }

        public void Update(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            this.SaveChanges();
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
        public void Rollback()
        {
            context
                .ChangeTracker
                .Entries()
                .ToList()
                .ForEach(x => x.Reload());
        }

		// Async section
		#region Async Methods

		public async Task AddAsync(T entity)
		{
			await DbSet.AddAsync(entity);
		}

		public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }
            
        public async Task<IEnumerable<T>> SearchForAsync(Expression<Func<T, bool>> predicate)
        {// was IQueryable
            var local = DbSet.Local.Where(predicate.Compile());
	        return local.Any()
                ? local
                : await DbSet.Where(predicate).ToArrayAsync().ConfigureAwait(false);
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            await this.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
        #endregion
    }
}