using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace EasyValidation
{
    internal static class ValidationObjects
    {
        static readonly Dictionary<Type, IEnumerable<PropertyValidator>> _validators = new Dictionary<Type, IEnumerable<PropertyValidator>>();

        public static bool AddObject(Type instanceType, IEnumerable<PropertyValidator> validators)
        {
            if (_validators.ContainsKey(instanceType))
                return false;

            _validators.Add(instanceType, validators);
            return true;
        }

        public static IEnumerable<PropertyValidator> GetValidators(Type instanceType)
        {
            if (_validators.TryGetValue(instanceType, out var validators))
                return validators;
            return Enumerable.Empty<PropertyValidator>();
        }
    }

    public static class ValidationObjects<T>
    {
        static readonly Dictionary<PropertyInfo, PropertyValidator> _validators = new Dictionary<PropertyInfo, PropertyValidator>();

        static ValidationObjects() => ValidationObjects.AddObject(typeof(T), _validators.Values);

        public static IEnumerable<PropertyValidator> Validators => _validators.Values;

        public static PropertyValidatorBuilder<T, TProperty> Property<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            var propertyInfo = Helper.GetPropertyInfo(expression);

            if (!_validators.TryGetValue(propertyInfo, out var validator))
                _validators.Add(propertyInfo, validator = new PropertyValidator(propertyInfo));

            return new PropertyValidatorBuilder<T, TProperty>(validator);
        }
    }
}
