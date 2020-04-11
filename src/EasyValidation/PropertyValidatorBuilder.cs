
using EasyValidation.Rules;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace EasyValidation
{
    public class PropertyValidatorBuilder<T, TProperty>
    {
        public PropertyValidatorBuilder(PropertyValidator validator)
        {
            Validator = validator;
        }

        public PropertyValidator Validator { get; }

        public PropertyValidatorBuilder<T, TProperty> WithMessage(string message)
        {
            Validator.Descriptor.ErrorMessage = message;
            return this;
        }

        public PropertyValidatorBuilder<T, TProperty> DisplayName(string name)
        {
            Validator.Descriptor.DisplayName = name;
            return this;
        }

        public PropertyValidatorBuilder<T, TProperty> Null(string message = null)
        {
            Validator.Rules.Add(new NullValidationRule { ErrorMessage = message });
            return this;
        }

        public PropertyValidatorBuilder<T, TProperty> Empty(string message = null)
        {
            Validator.Rules.Add(new EmptyValidationRule (default(TProperty)) { ErrorMessage = message });
            return this;
        }

        public PropertyValidatorBuilder<T, TProperty> Not(Action<PropertyValidatorBuilder<T, TProperty>> buildAction, string message = null)
        {
            var builder = CreateChildBuilder();
            buildAction?.Invoke(builder);
            if (builder.Validator.Rules.Count > 0)
                Validator.Rules.Add(new NotValidationRule(builder.Validator.Rules) { ErrorMessage = message });

            return this;
        }

        public PropertyValidatorBuilder<T, TProperty> NotNull(string message = "{PropertyName} 不能为Null。")
        {
            Validator.Rules.Add(new NotValidationRule(new NullValidationRule()) { ErrorMessage = message });
            return this;
        }

        public PropertyValidatorBuilder<T, TProperty> NotEmpty(string message = "{PropertyName} 不能为空。")
        {
            Validator.Rules.Add(new NotValidationRule(new EmptyValidationRule(default(TProperty))) { ErrorMessage = message });
            return this;
        }

        public PropertyValidatorBuilder<T, TProperty> Mutiple(Action<PropertyValidatorBuilder<T, TProperty>> buildAction, string message = null)
        {
            var builder = CreateChildBuilder();
            buildAction?.Invoke(builder);
            if (builder.Validator.Rules.Count > 0)
                Validator.Rules.Add(new MultipleValidationRule(builder.Validator) { ErrorMessage = message });

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

        public PropertyValidatorBuilder<T, TProperty> Must(Func<TProperty, bool> predicate, string message = null)
        {
            Check.IfNullThrow(predicate, "Cannot pass a null predicate to Must.");

            return Must((instance, propertyValue, context) => predicate(propertyValue), message);
        }

        public PropertyValidatorBuilder<T, TProperty> Must(Func<T, TProperty, bool> predicate, string message = null)
        {
            Check.IfNullThrow(predicate, "Cannot pass a null predicate to Must.");

            return Must((instance, propertyValue, context) => predicate(instance, propertyValue), message);
        }

        public PropertyValidatorBuilder<T, TProperty> Must(Func<T, TProperty, PropertyValidationContext, bool> predicate, string message = null)
        {
            Check.IfNullThrow(predicate, "Cannot pass a null predicate to Must.");

            Validator.Rules.Add(new PredicateValidationRule((instance, propertyValue, context) => predicate((T)instance, (TProperty)propertyValue, context)) { ErrorMessage = message });
            return this;
        }

        public PropertyValidatorBuilder<T, TProperty> Match(Func<T, string> expression, RegexOptions options = RegexOptions.None, string message = null)
        {
            Validator.Rules.Add(new RegularExpressionValidationRule(o => expression((T)o), options) { ErrorMessage = message });
            return this;
        }

        public PropertyValidatorBuilder<T, TProperty> Match(string pattern, RegexOptions options = RegexOptions.None, string message = null)
        {
            Validator.Rules.Add(new RegularExpressionValidationRule(pattern, options) { ErrorMessage = message });
            return this;
        }

        private PropertyValidatorBuilder<T, TProperty> CreateChildBuilder()
        {
            var validator = new PropertyValidator(Validator.Descriptor);
            return new PropertyValidatorBuilder<T, TProperty>(validator);
        }
    }
}
