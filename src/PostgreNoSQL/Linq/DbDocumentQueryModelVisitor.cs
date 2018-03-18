namespace PostgreNoSQL.Linq
{
    using Remotion.Linq;
    using Remotion.Linq.Clauses;

    /// <summary>
    ///     Provides an implementation of the Remotion.Linq.IQueryModelVisitor 
    ///     which automatically visits child items.
    /// </summary>
    public class DbDocumentQueryModelVisitor : QueryModelVisitorBase
    {
        private readonly CommandBuilder _cmdBuilder;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DbDocumentQueryModelVisitor" /> class.
        /// </summary>
        public DbDocumentQueryModelVisitor(CommandBuilder commandBuilder)
        {
            _cmdBuilder = Check.NotNull(commandBuilder, nameof(commandBuilder));
            _cmdBuilder.Append(" FROM ");
        }

        /// <summary>
        ///     Main entry point for visiting a given <paramref name="queryModel"/>.
        /// </summary>
        /// <param name="queryModel"></param>
        public override void VisitQueryModel(QueryModel queryModel)
        {
            queryModel.MainFromClause.Accept(this, queryModel);
            VisitBodyClauses (queryModel.BodyClauses, queryModel);
        }

        public override void VisitMainFromClause(MainFromClause fromClause, QueryModel queryModel)
        {
            _cmdBuilder.Append(fromClause.ItemType.Name + " " + fromClause.ItemName);
            base.VisitMainFromClause(fromClause, queryModel);
        }

        public override void VisitWhereClause(WhereClause whereClause, QueryModel queryModel, int index)
        {
            new WhereClauseExpressionVisitor(_cmdBuilder).Visit(whereClause.Predicate);
            base.VisitWhereClause(whereClause, queryModel, index);
        }
    }
}
