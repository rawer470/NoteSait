using System;
using System.Linq.Expressions;
using NoteSait.Data;
using NoteSait.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace NoteSait.Services;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly Context context;
    private DbSet<T> dbSet;
    public Repository(Context context)
    {
        this.context = context;
        this.dbSet = context.Set<T>();
    }
    public void Add(T entity) { dbSet.Add(entity); Save(); }
    public T Find(string id) { return dbSet.Find(id); }
    public void Remove(T entity) { dbSet.Remove(entity); Save(); }
    public void Save() { context.SaveChanges(); }
    public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        string includeProperties = null, bool isTracking = true)
    {
        IQueryable<T> query = dbSet;
        // есть ли фильтр
        if (filter != null)
        {
            // добавить ссылку на фильтр - параметр метода
            // + сформировать запрос
            query = query.Where(filter);
        }
        // свойства
        if (includeProperties != null)
        {
            foreach (var includeProp in includeProperties.Split(',')) { query = query.Include(includeProp); }
        }
        // сортировка
        if (orderBy != null) { query = orderBy(query); }
        if (!isTracking) { query = query.AsNoTracking(); }
        return query.ToList();
    }
    public T FirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null, bool isTracking = true)
    {
        IQueryable<T> query = dbSet;
        // есть ли фильтр
        if (filter != null)
        {
            // добавить ссылку на фильтр - параметр метода
            // + сформировать запрос
            query = query.Where(filter);
        }
        // свойства
        if (includeProperties != null)
        {
            foreach (var includeProp in includeProperties.Split(',')) { query = query.Include(includeProp); }
        }
        if (!isTracking) { query = query.AsNoTracking(); }
        return query.FirstOrDefault();
    }
    public void RemoveRange(IEnumerable<T> entity)
    {
        dbSet.RemoveRange(entity);
        Save();
    }
}
