namespace PostgreNoSQL.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using PostgreNoSQL.Internal;
    using Remotion.Linq.Clauses.Expressions;
    using Remotion.Linq.Parsing;

    public class WhereClauseExpressionVisitor : ThrowingExpressionVisitor
    {
        private static readonly Dictionary<ExpressionType, string> _operatorMap = new Dictionary<ExpressionType, string>
        {
            { ExpressionType.Equal, " = " },
            { ExpressionType.NotEqual, " <> " },
            { ExpressionType.GreaterThan, " > " },
            { ExpressionType.GreaterThanOrEqual, " >= " },
            { ExpressionType.LessThan, " < " },
            { ExpressionType.LessThanOrEqual, " <= " },
            { ExpressionType.AndAlso, " AND " },
            { ExpressionType.OrElse, " OR " },
            { ExpressionType.Add, " + " },
            { ExpressionType.Subtract, " - " },
            { ExpressionType.Multiply, " * " },
            { ExpressionType.Divide, " / " },
            { ExpressionType.Modulo, " % " },
            { ExpressionType.And, " & " },
            { ExpressionType.Or, " | " }
        };

        private readonly CommandBuilder _cmddBuilder = new CommandBuilder();

        protected override Expression VisitBinary(BinaryExpression expression)
        {
            _cmddBuilder.Append("(");

            Visit(expression.Left);

            _cmddBuilder.Append(_operatorMap[expression.NodeType]);

            Visit(expression.Right);

            _cmddBuilder.Append(")");

            return expression;
        }

        protected override Expression VisitQuerySourceReference(QuerySourceReferenceExpression expression)
        {
            _cmddBuilder.Append(expression.ReferencedQuerySource.ItemName);
            return expression;
        }

        protected override Expression VisitMember(MemberExpression expression)
        {
            Visit(expression.Expression);
            _cmddBuilder.Append($".{expression.Member.Name}");
            return expression;
        }

        protected override Expression VisitConstant(ConstantExpression expression)
        {
            string parameterName = _cmddBuilder.AddParameter(expression.Value);
            _cmddBuilder.Append($"@{parameterName}");
            return expression;
        }

        /// <summary>
        ///     Called when a LINQ expression type is not handled above.
        /// </summary>
        protected override Exception CreateUnhandledItemException<T>(T unhandledItem, string visitMethod) 
            => new NotSupportedException(visitMethod);
    }
}
