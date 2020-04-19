using System;
using System.Linq.Expressions;

namespace EasyValidation
{
    public class StrategyBuilder<T>
    {
        public StrategyBuilder()
        {
            Strategy = new Strategy();
        }

        public Strategy Strategy { get; }

        public StrategyBuilder<T> Include<TProperty>(params Expression<Func<T, TProperty>>[] expressions)
        {
            foreach (var expression in expressions)
            {
                var propertyInfo = Helper.GetPropertyInfo(expression);
                Check.IfNullThrow(propertyInfo);

                Strategy.IncludeProperties.Add(propertyInfo);
            }

            return this;
        }

        public StrategyBuilder<T> Exclude<TProperty>(params Expression<Func<T, TProperty>>[] expressions)
        {
            foreach (var expression in expressions)
            {
                var propertyInfo = Helper.GetPropertyInfo(expression);
                Check.IfNullThrow(propertyInfo);

                Strategy.ExcludeProperties.Add(propertyInfo);
            }

            return this;
        }
    }
}