using Core.Entities;
using Core.Interfaces;
using Core.Utils;
using Database.Utils;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace Database.BaseImplementations;

public class BaseRepository<T> : IBaseRepository<T> where T : class, IEntity, new()
{
    private readonly DbContext _dbContext;

    public BaseRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        var addedEntity = await _dbContext.Set<T>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return addedEntity.Entity;
    }

    public virtual async Task DeleteAsync(Guid uuid)
    {
        T entity = new T()
        { 
            UUID = uuid
        };

        _dbContext.Set<T>().Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public virtual async Task<T?> GetAsync(Guid uuid, IEnumerable<string>? toInclude = null)
    {
        IQueryable<T> query = _dbContext.Set<T>().Where(e => e.UUID == uuid).AsNoTracking();
        return await query.FirstOrDefaultAsync();
    }

    public virtual async Task<IReadOnlyCollection<T>> GetAsync(IEnumerable<Guid>? uuids, IEnumerable<string>? toInclude = null)
    {
        IQueryable<T> query = _dbContext.Set<T>().Where(e => uuids.Contains(e.UUID)).AsNoTracking();
        return await query.ToListAsync();
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
        var addedEntity = _dbContext.Set<T>().Update(entity);
        await _dbContext.SaveChangesAsync();
        return addedEntity.Entity;
    }

    private static void AddIncludedEntity<TEntity>(IQueryable<T> query, List<string> alreadyIncluded, string property) where TEntity : class
    {
        if (!property.Contains("."))
            return;
        
        int lastDot = property.LastIndexOf('.');
        string toInclude = property.Substring(0, lastDot);
        if (!alreadyIncluded.Contains(toInclude))
        {
            alreadyIncluded.Add(toInclude);
            query.Include(toInclude);
        }
    }

    public virtual async Task<IReadOnlyCollection<T>> GetAllAsync(IEnumerable<string>? toInclude = null)
    {
        return await _dbContext.Set<T>().ToListAsync();
    }

    public async Task<IReadOnlyCollection<T>> FindAsync(Filter? filter = null, string? orderBy = null, bool? sortDescending = null)
    {
        IQueryable<T> query = _dbContext.Set<T>();

        if (filter?.Conditions.Count != 0)
        {
            List<string> alreadyIncluded = new List<string>();
            Expression<Func<T, bool>> predicate = PredicateBuilder.True<T>();
            foreach (FilterCondition condition in filter.Conditions)
            {
                AddIncludedEntity<T>(query, alreadyIncluded, condition.Property);
                predicate = predicate.And(BuildPredicate<T>(condition));
            }

            query = query.Where(predicate).AsNoTracking();
        }
        if (!string.IsNullOrEmpty(orderBy))
        {
            bool descending = sortDescending ?? false;
            query = descending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
        }

        return await query.ToListAsync();
    }

    private Expression<Func<TEntity, bool>> BuildPredicate<TEntity>(FilterCondition filterCondition)
    {
        var parameter = Expression.Parameter(typeof(TEntity));
        MemberExpression memberExpression = null;

        if (filterCondition.Property.Contains('.'))
        {
            foreach (string property in filterCondition.Property.Split('.'))
            { 
                memberExpression = memberExpression is null ? Expression.Property(parameter, property)
                                                            : Expression.Property(memberExpression, property);
            }
        }
        else
        {
            memberExpression = Expression.Property(parameter, filterCondition.Property);
        }

        ConstantExpression value = Expression.Constant(filterCondition.Value, memberExpression.Type);

        switch (filterCondition.Condition)
        {
            case ComparisonCondition.Equals:
                return Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(memberExpression, value), parameter);

            case ComparisonCondition.NotEquals:
                return Expression.Lambda<Func<TEntity, bool>>(Expression.NotEqual(memberExpression, value), parameter);

            case ComparisonCondition.GreaterThanOrEqual:
                return Expression.Lambda<Func<TEntity, bool>>(Expression.GreaterThanOrEqual(memberExpression, value), parameter);

            case ComparisonCondition.GreaterThan:
                return Expression.Lambda<Func<TEntity, bool>>(Expression.GreaterThan(memberExpression, value), parameter);

            case ComparisonCondition.LessThanOrEqual:
                return Expression.Lambda<Func<TEntity, bool>>(Expression.LessThanOrEqual(memberExpression, value), parameter);

            case ComparisonCondition.LessThan:
                return Expression.Lambda<Func<TEntity, bool>>(Expression.LessThan(memberExpression, value), parameter);

            case ComparisonCondition.Contains:
                if (memberExpression.Type == typeof(string))
                {
                    MethodInfo? method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    MethodCallExpression containsExpression = Expression.Call(memberExpression, method, value);
                    return Expression.Lambda<Func<TEntity, bool>>(containsExpression, parameter);
                }
                else
                { 
                    throw new NotSupportedException("Contains support only string");
                }

            default:
                throw new InvalidOperationException($"Unknown condition {filterCondition.Condition}");

        }
    }
}
