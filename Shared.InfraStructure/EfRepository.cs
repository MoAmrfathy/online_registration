using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shared.InfraStructure
{
    public class EfRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        internal DbContext _context;
        internal DbSet<TEntity> _dbSet;
        private ILogger<EfRepository<TEntity>> _logger;
        public EfRepository(DbContext context, ILogger<EfRepository<TEntity>> logger)
        {
            _context = context;
            //_context.Database.SetCommandTimeout(600);
            _logger = logger;
            _dbSet = context.Set<TEntity>();
        }


        public async Task<IEnumerable<TEntity>> All(bool getDeleted = false)
        {
            //            if(!getDeleted && typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity))
            //)
            //            {
            //                return _dbSet.Where((x => ((ISoftDelete)x).DeleteDateTime == null));
            //            }
            return await _dbSet.ToListAsync();
        }

        public async Task<RepositertyReturn<IEnumerable<TEntity>>> AllInclude
        (PagingParameterModel paging = null, Expression<Func<TEntity, object>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = GetAllIncluding(includeProperties);
            RepositertyReturn<IEnumerable<TEntity>> ret = new RepositertyReturn<IEnumerable<TEntity>>();
            ret.Count = await query.CountAsync();
            if (orderBy != null)
            {
                query = query.OrderBy(orderBy);
            }

            if (paging != null)
            {
                if (paging.pageNumber != -1)
                {
                    query = query.Skip((paging.pageNumber - 1) * paging.pageSize).Take(paging.pageSize);
                }
            }

            ret.Data = await query.ToListAsync();

            return ret;
        }

        public async Task<RepositertyReturn<IEnumerable<TEntity>>> AllIncludeDescending
        (int page = -1, int quantity = 10, string orderBy = "",
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = GetAllIncluding(includeProperties);
            RepositertyReturn<IEnumerable<TEntity>> ret = new RepositertyReturn<IEnumerable<TEntity>>();
            ret.Count = await query.CountAsync();
            if (orderBy != "")
            {
                query = query.OrderByDescending(orderBy);
            }

            if (page != -1)
            {
                query = query.Skip((page - 1) * quantity).Take(quantity);
            }

            ret.Data = await query.ToListAsync();

            return ret;
        }

        public async Task<RepositertyReturn<IEnumerable<TEntity>>> AllInclude
            (string includeProperty, int page = -1, int quantity = 10, string orderBy = "")
        {
            var query = GetAllIncluding(includeProperty);
            RepositertyReturn<IEnumerable<TEntity>> ret = new RepositertyReturn<IEnumerable<TEntity>>();
            ret.Count = await query.CountAsync();
            if (orderBy != "")
            {
                query = query.OrderBy(orderBy);
            }

            if (page != -1)
            {
                query = query.Skip((page - 1) * quantity).Take(quantity);
            }

            ret.Data = await query.ToListAsync();
            return ret;
        }

        public async Task<RepositertyReturn<IEnumerable<TEntity>>> FindByInclude
        (Expression<Func<TEntity, bool>> predicate, PagingParameterModel paging = null,
            Expression<Func<TEntity, object>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = GetAllIncluding(includeProperties);
            query = query.Where(predicate);
            RepositertyReturn<IEnumerable<TEntity>> ret = new RepositertyReturn<IEnumerable<TEntity>>();
            ret.Count = await query.CountAsync();
            if (orderBy != null)
            {
                query = query.OrderByDescending(orderBy);
            }

            if (paging != null)
            {
                if (paging.pageNumber != -1)
                    query = query.Skip((paging.pageNumber - 1) * paging.pageSize).Take(paging.pageSize);
            }

            ret.Data = await query.ToListAsync();
            return ret;
        }

        public async Task<RepositertyReturn<IEnumerable<TEntity>>> FindByInclude
        (Expression<Func<TEntity, bool>> predicate,
            string includeProperties, PagingParameterModel paging = null,
            Expression<Func<TEntity, object>> orderBy = null)
        {
            var query = GetAllIncluding(includeProperties);

            query = query.Where(predicate);
            RepositertyReturn<IEnumerable<TEntity>> ret = new RepositertyReturn<IEnumerable<TEntity>>();
            ret.Count = await query.CountAsync();
            if (orderBy != null)
            {
                query = query.OrderBy(orderBy);
            }

            if (paging != null)
            {
                query = query.Skip((paging.pageNumber - 1) * paging.pageSize).Take(paging.pageSize);
            }

            ret.Data = await query.ToListAsync();
            return ret;
        }
        public async Task<RepositertyReturn<IEnumerable<TEntity>>> FindByIncludeDescending
        (Expression<Func<TEntity, bool>> predicate,
            string includeProperties, PagingParameterModel paging = null,
            Expression<Func<TEntity, object>> orderByDescending = null)
        {
            var query = GetAllIncluding(includeProperties);
            query = query.Where(predicate);
            RepositertyReturn<IEnumerable<TEntity>> ret = new RepositertyReturn<IEnumerable<TEntity>>();
            ret.Count = await query.CountAsync();
            if (orderByDescending != null)
            {
                query = query.OrderByDescending(orderByDescending);
            }

            if (paging != null)
            {
                query = query.Skip((paging.pageNumber - 1) * paging.pageSize).Take(paging.pageSize);
            }

            ret.Data = await query.ToListAsync();
            return ret;
        }
        // Special For Requests Order in Committee
        public async Task<RepositertyReturn<IEnumerable<TEntity>>> FindByIncludeWithMultipleOrders
          (Expression<Func<TEntity, bool>> predicate,
              string includeProperties, PagingParameterModel paging = null,
              Expression<Func<TEntity, object>> orderByDescending = null, Expression<Func<TEntity, object>> secondOrder = null, Expression<Func<TEntity, object>> thirdOrder = null, Expression<Func<TEntity, object>> fourthOrder = null)
        {
            var query = GetAllIncluding(includeProperties);
            query = query.Where(predicate);
            RepositertyReturn<IEnumerable<TEntity>> ret = new RepositertyReturn<IEnumerable<TEntity>>();
            ret.Count = await query.CountAsync();
            if (orderByDescending != null)
            {
                query = query.OrderBy(orderByDescending).ThenBy(secondOrder).ThenBy(thirdOrder).ThenBy(fourthOrder);
            }

            if (paging != null)
            {
                query = query.Skip((paging.pageNumber - 1) * paging.pageSize).Take(paging.pageSize);
            }

            ret.Data = await query.ToListAsync();
            return ret;
        }
        public async Task<RepositertyReturn<IEnumerable<TEntity>>> FindByIncludeWithMultipleOrders
       (Expression<Func<TEntity, bool>> predicate, PagingParameterModel paging = null,
           Expression<Func<TEntity, object>> firstOrder = null, Expression<Func<TEntity, object>> secondOrder = null, Expression<Func<TEntity, object>> thirdOrder = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = GetAllIncluding(includeProperties);
            query = query.Where(predicate);
            RepositertyReturn<IEnumerable<TEntity>> ret = new RepositertyReturn<IEnumerable<TEntity>>();
            ret.Count = await query.CountAsync();
            if (firstOrder != null)
            {
                query = query.OrderBy(firstOrder).ThenBy(secondOrder).ThenBy(thirdOrder);
            }

            if (paging != null)
            {
                query = query.Skip((paging.pageNumber - 1) * paging.pageSize).Take(paging.pageSize);
            }

            ret.Data = await query.ToListAsync();
            return ret;
        }

        public async Task<IEnumerable<TEntity>> FindByInclude
        (Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = GetAllIncluding(includeProperties);
            IEnumerable<TEntity> results = await query.Where(predicate).ToListAsync();
            return results;
        }

        public async Task<IEnumerable<TEntity>> FindByInclude
        (Expression<Func<TEntity, bool>> predicate,
            string includeProperties)
        {
            var query = GetAllIncluding(includeProperties);
            IEnumerable<TEntity> results = await query.Where(predicate).ToListAsync();
            return results;
        }

        private IQueryable<TEntity> GetAllIncluding
            (string includePropertiesString)
        {
            var includeProperties = includePropertiesString.Split(',');
            IQueryable<TEntity> queryable = _dbSet;
            var query = includeProperties.Aggregate
                (queryable, (current, includeProperty) => current.Include(includeProperty));
            return query;
        }

        private IQueryable<TEntity> GetAllIncluding
            (params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> queryable = _dbSet;
            var query = includeProperties.Aggregate
                (queryable, (current, includeProperty) => current.Include(includeProperty));
            return query;
        }

        //public async Task<IEnumerable<TEntity>> AllMultipleInclude(
        //    params Expression<Func<TEntity, object>>[] includeExpressions)
        //{
        //    var set = _context.Set<TEntity>().AsQueryable();

        //    foreach (var includeExpression in includeExpressions)
        //    {
        //        set = set.Include(includeExpression);
        //    }

        //    return set;
        //}

        public async Task<RepositertyReturn<IEnumerable<TEntity>>> FindBy(Expression<Func<TEntity, bool>> predicate,
            PagingParameterModel paging)
        {
            var query = _dbSet
                .Where(predicate);

            RepositertyReturn<IEnumerable<TEntity>> ret = new RepositertyReturn<IEnumerable<TEntity>>();
            ret.Count = await query.CountAsync();

            if (paging != null)
            {
                if (paging.pageNumber != -1)
                    query = query.Skip((paging.pageNumber - 1) * paging.pageSize).Take(paging.pageSize);
            }

            IEnumerable<TEntity> results = await query.ToListAsync();
            ret.Data = results;
            return ret;
        }

        public async Task<RepositertyReturn<IEnumerable<TEntity>>> FindByDescending(Expression<Func<TEntity, bool>> predicate,
            PagingParameterModel paging = null, Expression<Func<TEntity, object>> orderByDescending = null)
        {
            var query = _dbSet
                .Where(predicate);

            RepositertyReturn<IEnumerable<TEntity>> ret = new RepositertyReturn<IEnumerable<TEntity>>();
            ret.Count = await query.CountAsync();

            if (orderByDescending != null)
            {
                query = query.OrderByDescending(orderByDescending);
            }

            if (paging != null)
            {
                query = query.Skip((paging.pageNumber - 1) * paging.pageSize).Take(paging.pageSize);
            }

            IEnumerable<TEntity> results = await query.ToListAsync();
            ret.Data = results;
            return ret;
        }

        public async Task<IEnumerable<TEntity>> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var query = _dbSet
                    .Where(predicate);

                IEnumerable<TEntity> results = await query.ToListAsync();
                return results;
            }
            catch (Exception ex)
            {
                IEnumerable<TEntity> results = await _dbSet
                    .Where(predicate).ToListAsync();
                return results;
            }
        }
        public async Task<TEntity> FindByKeyMultipleInclude(int id, String predicate)
        {
            IQueryable<TEntity> query = null;
            Expression<Func<TEntity, bool>> lambda = Utilities.BuildLambdaForFindByKey<TEntity>(id);
            foreach (var includeProperty in predicate.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = _dbSet.Include(predicate);
            }
            return await query.Where(lambda).SingleOrDefaultAsync(lambda);
        }
        public async Task<TEntity> FindByKey(int id)
        {
            Expression<Func<TEntity, bool>> lambda = Utilities.BuildLambdaForFindByKey<TEntity>(id);
            return await _dbSet.SingleOrDefaultAsync(lambda);
        }

        public async Task<TEntity> FindByKey(Guid id)
        {
            Expression<Func<TEntity, bool>> lambda = Utilities.BuildLambdaForFindByKey<TEntity>(id);
            return await _dbSet.SingleOrDefaultAsync(lambda);
        }

        public async Task<TEntity> FindByKey(int id, Expression<Func<TEntity, object>> predicate)
        {
            Expression<Func<TEntity, bool>> lambda = Utilities.BuildLambdaForFindByKey<TEntity>(id);
            return await _dbSet.Include(predicate).SingleOrDefaultAsync(lambda);
        }

        public async Task<TEntity> FindByKey(int id, string includeProperty)
        {
            Expression<Func<TEntity, bool>> lambda = Utilities.BuildLambdaForFindByKey<TEntity>(id);
            return await _dbSet.Include(includeProperty).SingleOrDefaultAsync(lambda);
        }

        public async Task<TEntity> FindByKey(Guid id, string includeProperty)
        {
            Expression<Func<TEntity, bool>> lambda = Utilities.BuildLambdaForFindByKey<TEntity>(id);
            return await _dbSet.Include(includeProperty).SingleOrDefaultAsync(lambda);
        }

        public async Task<TEntity> FindByKey(Guid id, Expression<Func<TEntity, object>> predicate)
        {
            Expression<Func<TEntity, bool>> lambda = Utilities.BuildLambdaForFindByKey<TEntity>(id);
            return await _dbSet.Include(predicate).SingleOrDefaultAsync(lambda);
        }

        public async Task Insert(TEntity entity)
        {
            _dbSet.Add(entity);
            _context.ChangeTracker.DetectChanges();
            _logger.LogDebug(_context.ChangeTracker.DebugView.LongView);
            await _context.SaveChangesAsync();
        }

        public async Task InsertRange(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities.ToList());
            await _context.SaveChangesAsync();
        }

        public class PropertyCopier<TParent, TChild> where TParent : class
            where TChild : class
        {
            public static void Copy(TParent parent, TChild child)
            {
                var parentProperties = parent.GetType().GetProperties();
                var childProperties = child.GetType().GetProperties();

                foreach (var parentProperty in parentProperties.Where(x => x.SetMethod != null))
                {
                    foreach (var childProperty in childProperties)
                    {
                        if (parentProperty.Name == childProperty.Name &&
                            parentProperty.PropertyType == childProperty.PropertyType)
                        {
                            childProperty.SetValue(child, parentProperty.GetValue(parent));
                            break;
                        }
                    }
                }
            }
        }

        [ObsoleteAttribute]
        public async Task Update(TEntity updatedEntity, TEntity currentEntity)
        {
            PropertyCopier<TEntity, TEntity>.Copy(updatedEntity, currentEntity);



            await _context.SaveChangesAsync();
        }

        public async Task Update()
        {

            await _context.SaveChangesAsync();
        }
        public async Task Update(TEntity updatedEntity)
        {
            await _context.SaveChangesAsync();
        }
        public async Task UpdateRange(IEnumerable<TEntity> updatedEntities)
        {
            //_dbSet.AddRange(updatedEntities.ToList());
            await _context.SaveChangesAsync();
        }


        //public async Task Delete(Guid id)
        //{
        //    var entity = await FindByKey(id);
        //    _dbSet.Remove(entity);
        //    await _context.SaveChangesAsync();
        //}

        public async Task Delete(int id)
        {
            var entity = await FindByKey(id);
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<object> Max(Expression<Func<TEntity, object>> expression, object defaultalue)
        {
            var ret = await _dbSet.MaxAsync(expression);
            if (ret == null)
            {
                return defaultalue;
            }

            return ret;
        }
    }
}