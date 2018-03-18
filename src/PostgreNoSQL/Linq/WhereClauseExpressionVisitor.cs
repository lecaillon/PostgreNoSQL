namespace PostgreNoSQL.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
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

        private readonly CommandBuilder _cmdBuilder;

        public WhereClauseExpressionVisitor(CommandBuilder commandBuilder)
        {
            _cmdBuilder = Check.NotNull(commandBuilder, nameof(commandBuilder));
            _cmdBuilder.Append(" WHERE ");
        }

        protected override Expression VisitBinary(BinaryExpression expression)
        {
            _cmdBuilder.Append("(");

            Visit(expression.Left);

            _cmdBuilder.Append(_operatorMap[expression.NodeType]);

            Visit(expression.Right);

            _cmdBuilder.Append(")");

            return expression;
        }

        protected override Expression VisitQuerySourceReference(QuerySourceReferenceExpression expression)
        {
            _cmdBuilder.Append(expression.ReferencedQuerySource.ItemName);
            return expression;
        }

        protected override Expression VisitMember(MemberExpression expression)
        {
            Visit(expression.Expression);
            _cmdBuilder.Append($".{expression.Member.Name}");
            return expression;
        }

        protected override Expression VisitConstant(ConstantExpression expression)
        {
            string parameterName = _cmdBuilder.AddParameter(expression.Value);
            _cmdBuilder.Append($"@{parameterName}");
            return expression;
        }

        /// <summary>
        ///     Called when a LINQ expression type is not handled above.
        /// </summary>
        protected override Exception CreateUnhandledItemException<T>(T unhandledItem, string visitMethod) 
            => new NotSupportedException(visitMethod);
    }
}
