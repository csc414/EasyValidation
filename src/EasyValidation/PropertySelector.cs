using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace EasyValidation
{
    public interface IPropertySelect
    {
        IEnumerable<PropertyValidator> Select(IEnumerable<PropertyValidator> validators);
    }

    public abstract class PropertySelector : IPropertySelect
    {
        protected ICollection<PropertyInfo> IncludeProperties { get; } = new HashSet<PropertyInfo>();

        protected ICollection<PropertyInfo> ExcludeProperties { get; } = new HashSet<PropertyInfo>();

        IEnumerable<PropertyValidator> IPropertySelect.Select(IEnumerable<PropertyValidator> validators)
        {
            if (IncludeProperties.Any() && ExcludeProperties.Any())
                throw new ArgumentException("Include or Exclude, you can only choose one.");

            if (IncludeProperties.Any())
                validators = validators.Where(o => IncludeProperties.Contains(o.Descriptor.PropertyInfo));
            else if (ExcludeProperties.Any())
                validators = validators.Where(o => !ExcludeProperties.Contains(o.Descriptor.PropertyInfo));

            return validators;
        }
    }

    public class PropertySelector<T> : PropertySelector
    {
        public PropertySelector<T> Include<TProperty>(params Expression<Func<T, TProperty>>[] expressions)
        {
            foreach (var expression in expressions)
            {
                var propertyInfo = Helper.GetPropertyInfo(expression);
                Check.IfNullThrow(propertyInfo);

                IncludeProperties.Add(propertyInfo);
            }

            return this;
        }

        public PropertySelector<T> Exclude<TProperty>(params Expression<Func<T, TProperty>>[] expressions)
        {
            foreach (var expression in expressions)
            {
                var propertyInfo = Helper.GetPropertyInfo(expression);
                Check.IfNullThrow(propertyInfo);

                ExcludeProperties.Add(propertyInfo);
            }

            return this;
        }
    }
}
