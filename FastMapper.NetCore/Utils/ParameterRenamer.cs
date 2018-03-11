using System.Linq.Expressions;

namespace FastMapper.NetCore.Utils
{
    public class ParameterRenamer : ExpressionVisitor
    {
        private string _expName;
        private ParameterExpression _parameterExpression;

        public Expression Rename(Expression expression, ParameterExpression parameterExpression)
        {
            _parameterExpression = parameterExpression;
            return Visit(expression);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (_expName == null)
                _expName = node.Name;

            return node.Name == _expName && node.Type == _parameterExpression.Type ? _parameterExpression : node;
        }
    }
}