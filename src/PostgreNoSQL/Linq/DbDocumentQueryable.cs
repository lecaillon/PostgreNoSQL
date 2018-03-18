namespace PostgreNoSQL.Linq
{
    using System.Linq;
    using System.Linq.Expressions;
    using Remotion.Linq;

    /// <summary>
    ///     Provides the main entry point to a LINQ query.
    /// </summary>
    public class DbDocumentQueryable<TResult> : QueryableBase<TResult>
    {
        /// <summary>
        ///     Intializes a new instance of the <see cref="DbDocumentQueryable{TResult}"/> class.
        ///     Called when a <see cref="DbDocument{TEntity}"/> is instantiated.
        /// </summary>
        public DbDocumentQueryable(IQueryProvider provider) 
            : base(Check.NotNull(provider, nameof(provider)))
        {
        }

        /// <summary>
        ///     Intializes a new instance of the <see cref="DbDocumentQueryable{TResult}"/> class.
        ///     Called indirectly by LINQ's query methods, just pass to base.
        /// </summary>
        public DbDocumentQueryable(IQueryProvider provider, Expression expression)
            : base(Check.NotNull(provider, nameof(provider)),
                   Check.NotNull(expression, nameof(expression)))
        {
        }
    }
}
