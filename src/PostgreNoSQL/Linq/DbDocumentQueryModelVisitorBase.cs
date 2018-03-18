namespace PostgreNoSQL.Linq
{
    using System;
    using Remotion.Linq;
    using Remotion.Linq.Clauses;

    public class DbDocumentQueryModelVisitorBase : QueryModelVisitorBase
    {
        public override void VisitQueryModel(QueryModel queryModel)
        {
            queryModel.MainFromClause.Accept(this, queryModel);
            VisitBodyClauses (queryModel.BodyClauses, queryModel);
        }

        public override void VisitMainFromClause(MainFromClause fromClause, QueryModel queryModel)
        {
            string alias = fromClause.ItemName;
            Type documentType = fromClause.ItemType;

            base.VisitMainFromClause(fromClause, queryModel);
        }

        public override void VisitWhereClause(WhereClause whereClause, QueryModel queryModel, int index)
        {
            new WhereClauseExpressionVisitor().Visit(whereClause.Predicate);
            base.VisitWhereClause(whereClause, queryModel, index);
        }
    }
}
