using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Domain
{
    public interface IRepository<TEntity> where TEntity : class
    {
        
        Task<IEnumerable<TEntity>> All(bool getDeleted = false);
        Task<object> Max(Expression<Func<TEntity, object>> expression, object defaultalue);
        Task<RepositertyReturn<IEnumerable<TEntity>>> AllInclude
        (PagingParameterModel paging = null, Expression<Func<TEntity, object>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includeProperties);
        Task<RepositertyReturn<IEnumerable<TEntity>>> AllIncludeDescending
       (int page = -1, int quantity = 10, string orderBy = "",
           params Expression<Func<TEntity, object>>[] includeProperties);

        Task<RepositertyReturn<IEnumerable<TEntity>>> FindBy(Expression<Func<TEntity, bool>> predicate,
            PagingParameterModel paging);

        Task<TEntity> FindByKey(int id);
        Task<TEntity> FindByKey(Guid id);
        Task<TEntity> FindByKey(int id, Expression<Func<TEntity, object>> predicate);
        Task<TEntity> FindByKey(int id, string includeProperty);
        Task<TEntity> FindByKey(Guid id, string includeProperty);
        Task<TEntity> FindByKey(Guid id, Expression<Func<TEntity, object>> predicate);
        Task Insert(TEntity entity);
        Task InsertRange(IEnumerable<TEntity> entities);
        Task Update(TEntity updatedEntity, TEntity currentEntity);
        Task Update();
        Task Update(TEntity updatedEntity);
        Task Delete(int id);

        Task<RepositertyReturn<IEnumerable<TEntity>>> FindByInclude
        (Expression<Func<TEntity, bool>> predicate,
            string includeProperties, PagingParameterModel paging = null, Expression<Func<TEntity, object>> orderBy = null);

        Task<RepositertyReturn<IEnumerable<TEntity>>> FindByInclude
        (Expression<Func<TEntity, bool>> predicate, PagingParameterModel paging = null,
            Expression<Func<TEntity, object>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includeProperties);

        Task<IEnumerable<TEntity>> FindBy(Expression<Func<TEntity, bool>> predicate);
    }
}
