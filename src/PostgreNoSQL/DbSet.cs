namespace PostgreNoSQL
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class DbSet<TEntity> : IQueryable<TEntity>, IOrderedQueryable<TEntity>, IQueryProvider where TEntity : class
    {
        private Expression _expression;

        #region IQueryable<TEntity>

        Type IQueryable.ElementType => typeof(TEntity);

        Expression IQueryable.Expression => Expression.Constant(this);

        IQueryProvider IQueryable.Provider => this;

        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator() => (this as IQueryable).Provider.Execute<IEnumerator<TEntity>>(_expression);

        #endregion

        #region IQueryProvider

        IEnumerator IEnumerable.GetEnumerator() => (IEnumerator<TEntity>)(this as IQueryable).GetEnumerator();

        IQueryable IQueryProvider.CreateQuery(Expression expression) => (this as IQueryProvider).CreateQuery<TEntity>(expression);

        IQueryable<TElement> IQueryProvider.CreateQuery<TElement>(Expression expression)
        {
            _expression = expression;

            return (IOrderedQueryable<TElement>)this;
        }

        object IQueryProvider.Execute(Expression expression) => (this as IQueryProvider).Execute<IEnumerator<TEntity>>(expression);

        TResult IQueryProvider.Execute<TResult>(Expression expression)
        {
            return (TResult) (new List<TEntity>() as IList<TEntity>).GetEnumerator();
        }

        #endregion
    }
}

//public virtual EntityEntry<TEntity> Add(TEntity entity) => throw new NotImplementedException();
