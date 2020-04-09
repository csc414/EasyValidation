using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace EasyValidation
{
    public static class ValidationObjects
    {
        static readonly Dictionary<Type, IEnumerable<PropertyValidator>> _validators = new Dictionary<Type, IEnumerable<PropertyValidator>>();

        internal static bool AddObject(Type instanceType, IEnumerable<PropertyValidator> validators)
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

    public static class ValidationObjects<TEntity>
    {
        static readonly Dictionary<PropertyInfo, PropertyValidator> _validators = new Dictionary<PropertyInfo, PropertyValidator>();

        static ValidationObjects() => ValidationObjects.AddObject(typeof(TEntity), _validators.Values);

        public static PropertyValidatorBuilder Property<TProperty>(Expression<Func<TEntity, TProperty>> expression)
        {
            return Property(GetPropertyInfo(expression));
        }
        
        public static PropertyValidatorBuilder Property(string name)
        {
            return Property(typeof(TEntity).GetRuntimeProperty(name));
        }

        static PropertyValidatorBuilder Property(PropertyInfo propertyInfo)
        {
            Check.IfNullThrow(propertyInfo, message: "Property not found.");

            if (!_validators.TryGetValue(propertyInfo, out var validator))
                _validators.Add(propertyInfo, validator = new PropertyValidator(propertyInfo));

            return new PropertyValidatorBuilder(validator);
        }

        static PropertyInfo GetPropertyInfo(LambdaExpression expression)
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
