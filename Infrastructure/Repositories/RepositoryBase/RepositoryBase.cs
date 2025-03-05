using Infrastructure.Entities.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ParameterRebinder : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> _map;

        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            _map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (_map.TryGetValue(node, out var replacement))
            {
                return replacement;
            }
            return base.VisitParameter(node);
        }

        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }
    }
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly DBContext _db;
        public RepositoryBase(DBContext dbContext)
        {
            _db = dbContext;
        }
        //hiển thị thành các pages theo điều kiện
        public async Task<List<T>> GetAll(int? pageSize, int? pageIndex, Expression<Func<T, bool>> expression)
        {
            var query = _db.Set<T>().Where(expression).AsQueryable();
            var pageCount = query.Count();
            query = query.Skip((pageIndex.Value - 1) * pageSize.Value)
            .Take(pageSize.Value).AsNoTracking();
            return await query.ToListAsync();
        }
        //hiển thị thành các pages không theo điều kiện
        public async Task<List<T>> GetAll(int? pageSize, int? pageIndex)
        {
            var query = _db.Set<T>().AsQueryable();
            var pageCount = query.Count();

            query = query.Skip(((pageIndex ?? 1) - 1) * pageSize ?? 10)
            .Take(pageSize.Value).AsNoTracking();
            return await query.ToListAsync();
        }
        //Get theo các trường ( các trường dùng dưới dạng Expression)
        public async Task<List<T>> GetByCondition(Expression<Func<T, bool>> expression)
        {
            var a = await _db.Set<T>().Where(expression).AsNoTracking().ToListAsync();
            return a;
        }
        public async Task<List<T>> GetByConditions(List<Expression<Func<T, bool>>> expression)
        {
            if (expression == null || expression.Count == 0)
            {
                return await _db.Set<T>().AsNoTracking().ToListAsync();
            }
            var combinedCondition = expression[0];
            foreach (var condition in expression.Skip(1))
            {
                combinedCondition = Expression.Lambda<Func<T, bool>>(
                    Expression.AndAlso(combinedCondition.Body, condition.Body),
                    combinedCondition.Parameters);
            }
            return await _db.Set<T>().Where(combinedCondition).AsNoTracking().ToListAsync();
        }

        public async System.Threading.Tasks.Task UpdateAsync(T entity)
        {
            _db.Set<T>().Update(entity);
            await _db.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeleteAsync(T entity)
        {
            _db.Set<T>().Remove(entity);
            await _db.SaveChangesAsync();

        }

        public async System.Threading.Tasks.Task CreateAsync(T entity)
        {
            _db.Set<T>().Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<T> GetById(Expression<Func<T, bool>> expression)
        {
            var a = await _db.Set<T>().Where(expression).FirstOrDefaultAsync();
            return a;
        }
        public async Task<T> GetByConditionId(List<Expression<Func<T, bool>>> expressions)
        {
            if (expressions == null || expressions.Count == 0)
            {
                return await _db.Set<T>().FirstOrDefaultAsync();
            }

            var parameter = Expression.Parameter(typeof(T), "e");
            Expression combinedExpression = ParameterRebinder.ReplaceParameters(
                new Dictionary<ParameterExpression, ParameterExpression> { { expressions[0].Parameters[0], parameter } },
                expressions[0].Body
            );

            foreach (var condition in expressions.Skip(1))
            {
                var replacedBody = ParameterRebinder.ReplaceParameters(
                    new Dictionary<ParameterExpression, ParameterExpression> { { condition.Parameters[0], parameter } },
                    condition.Body
                );
                combinedExpression = Expression.AndAlso(combinedExpression, replacedBody);
            }

            var combinedLambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
            return await _db.Set<T>().Where(combinedLambda).FirstOrDefaultAsync();
        }
        public async Task<T> GetById(int id)
        {
            var a = await _db.Set<T>().FindAsync(id);
            return a;
        }

        public async Task<int> CountAsync()
        {
            var query = await _db.Set<T>().CountAsync();
            return query;
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> expression)
        {
            var query = await _db.Set<T>().Where(expression).CountAsync();
            return query;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            var query = _db.Set<T>().ToList();
            return query;
        }

        public Task<IEnumerable<T>> GetAll(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<T> FindByName(Expression<Func<T, bool>> expression)
        {
            var query = _db.Set<T>().Where(expression).FirstOrDefault();
            return query;
        }

        public async Task<IQueryable<T>> GetAllAsQueryable()
        {
            var query = _db.Set<T>().AsQueryable();
            return query;
        }
    }
}
