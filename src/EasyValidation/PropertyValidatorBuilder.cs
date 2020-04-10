
using EasyValidation.Rules;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EasyValidation
{
    public class PropertyValidatorBuilder<T, TProperty>
    {
        public PropertyValidatorBuilder(PropertyValidator validator)
        {
            Validator = validator;
        }

        public PropertyValidator Validator { get; }

        public PropertyValidatorBuilder<T, TProperty> DisplayName(string name)
        {
            Validator.Descriptor.DisplayName = name;
            return this;
        }

        public PropertyValidatorBuilder<T, TProperty> NotNull(string message = null)
        {
            Validator.Rules.Add(new NotNullValidationRule { ErrorMessage = message });
            return this;
        }

        public PropertyValidatorBuilder<T, TProperty> NotEmpty(string message = null)
        {
            Validator.Rules.Add(new NotEmptyValidationRule (default(TProperty)) { ErrorMessage = message });
            return this;
        }

        public PropertyValidatorBuilder<T, TProperty> Mutiple(Action<PropertyValidatorBuilder<T, TProperty>> builder, string message = null)
        {
            var validator = new PropertyValidator(Validator.Descriptor);
            builder?.Invoke(new PropertyValidatorBuilder<T, TProperty>(validator));
            if (validator.Rules.Count > 0)
                Validator.Rules.Add(new MultipleValidationRule(validator) { ErrorMessage = message });

            return this;
        }

        public PropertyValidatorBuilder<T, TProperty> Equal(TProperty value, IEqualityComparer comparer = null, string message = null)
        {
            if (comparer == null && typeof(TProperty) == typeof(string))
                comparer = StringComparer.Ordinal;

            Validator.Rules.Add(new EqualValidationRule(value, comparer) { ErrorMessage = message });
            return this;
        }

        public PropertyValidatorBuilder<T, TProperty> Equal(Expression<Func<T, TProperty>> expression, IEqualityComparer comparer = null, string message = null)
        {
            if (comparer == null && typeof(TProperty) == typeof(string))
                comparer = StringComparer.Ordinal;

            Validator.Rules.Add(new EqualValidationRule(expression.Compile(), comparer) { ErrorMessage = message });
            return this;
        }
    }
}
