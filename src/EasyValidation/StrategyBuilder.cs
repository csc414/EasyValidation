using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace EasyValidation
{
    public class StrategyBuilder<T>
    {
        private readonly ICollection<PropertyInfo> _includeProperties = new HashSet<PropertyInfo>();

        private readonly ICollection<PropertyInfo> _excludeProperties = new HashSet<PropertyInfo>();

        public StrategyBuilder<T> Include<TProperty>(params Expression<Func<T, TProperty>>[] expressions)
        {
            foreach (var expression in expressions)
            {
                var propertyInfo = Helper.GetPropertyInfo(expression);
                Check.IfNullThrow(propertyInfo);

                _includeProperties.Add(propertyInfo);
            }
          
            return this;
        }

        public StrategyBuilder<T> Exclude<TProperty>(params Expression<Func<T, TProperty>>[] expressions)
        {
            foreach (var expression in expressions)
            {
                var propertyInfo = Helper.GetPropertyInfo(expression);
                Check.IfNullThrow(propertyInfo);

                _includeProperties.Add(propertyInfo);
            }

            return this;
        }

        public Strategy Build()
        {
            return new Strategy { IncludeProperties = _includeProperties, ExcludeProperties = _excludeProperties };
        }
    }
}
