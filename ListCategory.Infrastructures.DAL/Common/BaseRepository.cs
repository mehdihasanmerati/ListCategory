using ListCategory.Core.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace ListCategory.DataAccess.Common
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, new()
    {
        private readonly ApplicationDbContext context;

        public BaseRepository(ApplicationDbContext context)
        {
            this.context = context;
            context.Products.Include(u => u.Category).Include(u => u.CategoryId);
        }
        public void Add(T entity)
        {
            context.Set<T>().Add(entity);
            context.SaveChanges();
        }

        public T Get(Expression<Func<T, bool>> fillter, string? includeProperties = null)
        {
            IQueryable<T> query = context.Set<T>();
            //Category,CategoryId
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includProp in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includProp);
                }
            }
            return query.Where(fillter).FirstOrDefault();
        }

        public IEnumerable<T> GetAll(string? includeProperties = null)
        {
            IQueryable<T> query = context.Set<T>();
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includProp in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includProp);
                }
            }
            return query.ToList();
        }

        public void Remove(T entity)
        {
            context.Set<T>().Remove(entity);
            context.SaveChanges();
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            context.Set<T>().RemoveRange(entity);
            context.SaveChanges();
        }
    }
}
