namespace PostgreNoSQL.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Remotion.Linq;

    public class DbDocumentQueryExecutor : IQueryExecutor
    {
        /// <summary>
        ///     Executes a query with a collection result.
        /// </summary>
        public IEnumerable<TResult> ExecuteCollection<TResult>(QueryModel queryModel)
        {
            new DbDocumentQueryModelVisitorBase().VisitQueryModel(queryModel);
            throw new NotImplementedException();
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
