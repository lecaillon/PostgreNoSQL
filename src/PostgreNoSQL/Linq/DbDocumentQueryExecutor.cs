namespace PostgreNoSQL.Linq
{
    using System.Collections.Generic;
    using System.Linq;
    using Remotion.Linq;

    public class DbDocumentQueryExecutor : IQueryExecutor
    {
        private readonly DbContext _context;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DbDocumentQueryExecutor" /> class.
        /// </summary>
        public DbDocumentQueryExecutor(DbContext context)
        {
            _context = Check.NotNull(context, nameof(context));
            CommandBuilder = new CommandBuilder();
        }

        public CommandBuilder CommandBuilder { get; }

        /// <summary>
        ///     Executes a query with a collection result.
        /// </summary>
        public IEnumerable<TResult> ExecuteCollection<TResult>(QueryModel queryModel)
        {
            new DbDocumentQueryModelVisitor(CommandBuilder).VisitQueryModel(queryModel);

            return null;
        }

        /// <summary>
        ///     Executes a query with a scalar result, 
        ///     i.e. a query that ends with a result operator such as Count, Sum, or Average.
        /// </summary>
        public TResult ExecuteScalar<TResult>(QueryModel queryModel)
        {
            return ExecuteCollection<TResult>(queryModel).Single();
        }

        /// <summary>
        ///     Executes a query with a single result object, 
        ///     i.e. a query that ends with a result operator such as First, Last, Single, Min, or Max.
        /// </summary>
        public TResult ExecuteSingle<TResult>(QueryModel queryModel, bool returnDefaultWhenEmpty)
        {
            return returnDefaultWhenEmpty ? ExecuteCollection<TResult>(queryModel).SingleOrDefault()
                                          : ExecuteCollection<TResult>(queryModel).Single();
        }
    }
}
