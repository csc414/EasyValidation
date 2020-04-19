using System.Linq.Expressions;
using System.Reflection;

namespace EasyValidation
{
    internal static class Helper
    {
        public static PropertyInfo GetPropertyInfo(LambdaExpression expression)
        {
            MemberExpression memberExpression = null;

            if (expression.Body is UnaryExpression unaryExp)
                memberExpression = (MemberExpression)unaryExp.Operand;
            else if (expression.Body is MemberExpression memberExp)
                memberExpression = memberExp;

            if (memberExpression == null || memberExpression.Member.MemberType != MemberTypes.Property)
                return null;

            return (PropertyInfo)memberExpression.Member;
        }
    }
}