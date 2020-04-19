using EasyValidation.Rules;
using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace EasyValidation
{
    public static class PropertyValidatorBuilderExtensions
    {
        public static PropertyValidatorBuilder<T, TProperty> WithMessage<T, TProperty>(this PropertyValidatorBuilder<T, TProperty> builder, string message)
        {
            builder.Validator.Descriptor.ErrorMessage = message;
            return builder;
        }

        public static PropertyValidatorBuilder<T, TProperty> DisplayName<T, TProperty>(this PropertyValidatorBuilder<T, TProperty> builder, string name)
        {
            builder.Validator.Descriptor.DisplayName = name;
            return builder;
        }

        public static PropertyValidatorBuilder<T, TProperty> Null<T, TProperty>(this PropertyValidatorBuilder<T, TProperty> builder, string message = null) where TProperty : class
        {
            builder.Validator.Rules.Add(new NullValidationRule { ErrorMessage = message });
            return builder;
        }

        public static PropertyValidatorBuilder<T, TProperty?> Null<T, TProperty>(this PropertyValidatorBuilder<T, TProperty?> builder, string message = null) where TProperty : struct
        {
            builder.Validator.Rules.Add(new NullValidationRule { ErrorMessage = message });
            return builder;
        }

        public static PropertyValidatorBuilder<T, TProperty> Empty<T, TProperty>(this PropertyValidatorBuilder<T, TProperty> builder, string message = null)
        {
            builder.Validator.Rules.Add(new EmptyValidationRule(default(TProperty)) { ErrorMessage = message });
            return builder;
        }

        public static PropertyValidatorBuilder<T, TProperty> Not<T, TProperty>(this PropertyValidatorBuilder<T, TProperty> builder, Action<PropertyValidatorBuilder<T, TProperty>> buildAction, string message = null)
        {
            var childBuilder = builder.CreateChildBuilder();
            buildAction?.Invoke(childBuilder);
            if (childBuilder.Validator.Rules.Count > 0)
                builder.Validator.Rules.Add(new NotValidationRule(childBuilder.Validator.Rules) { ErrorMessage = message });

            return builder;
        }

        public static PropertyValidatorBuilder<T, TProperty> NotNull<T, TProperty>(this PropertyValidatorBuilder<T, TProperty> builder, string message = "{PropertyName} 不能为Null。") where TProperty : class
        {
            builder.Validator.Rules.Add(new NotValidationRule(new NullValidationRule()) { ErrorMessage = message });
            return builder;
        }

        public static PropertyValidatorBuilder<T, TProperty?> NotNull<T, TProperty>(this PropertyValidatorBuilder<T, TProperty?> builder, string message = "{PropertyName} 不能为Null。") where TProperty : struct
        {
            builder.Validator.Rules.Add(new NotValidationRule(new NullValidationRule()) { ErrorMessage = message });
            return builder;
        }

        public static PropertyValidatorBuilder<T, TProperty> NotEmpty<T, TProperty>(this PropertyValidatorBuilder<T, TProperty> builder, string message = "{PropertyName} 不能为空。")
        {
            builder.Validator.Rules.Add(new NotValidationRule(new EmptyValidationRule(default(TProperty))) { ErrorMessage = message });
            return builder;
        }

        public static PropertyValidatorBuilder<T, TProperty> Mutiple<T, TProperty>(this PropertyValidatorBuilder<T, TProperty> builder, Action<PropertyValidatorBuilder<T, TProperty>> buildAction, string message = null)
        {
            var childbuilder = builder.CreateChildBuilder();
            buildAction?.Invoke(childbuilder);
            if (childbuilder.Validator.Rules.Count > 0)
                builder.Validator.Rules.Add(new MultipleValidationRule(childbuilder.Validator) { ErrorMessage = message });

            return builder;
        }

        public static PropertyValidatorBuilder<T, TProperty> When<T, TProperty>(this PropertyValidatorBuilder<T, TProperty> builder, Func<T, bool> predicate, Action<PropertyValidatorBuilder<T, TProperty>> buildAction)
        {
            Check.IfNullThrow(predicate, "Cannot pass a null predicate to When.");

            var childbuilder = builder.CreateChildBuilder();
            buildAction?.Invoke(childbuilder);
            if (childbuilder.Validator.Rules.Count > 0)
            {
                var validator = childbuilder.Validator;
                builder.Validator.Rules.Add(new PredicateValidationRule((instance, propertyValue, rule, context) =>
                {
                    if (predicate((T)instance))
                    {
                        var failure = validator.Validate(context.Context);
                        if (failure != null)
                        {
                            rule.ErrorMessage = failure.ErrorMessage;
                            return false;
                        }
                    }
                    return true;
                }));
            }
            return builder;
        }

        public static PropertyValidatorBuilder<T, TProperty> Equal<T, TProperty>(this PropertyValidatorBuilder<T, TProperty> builder, TProperty value, IEqualityComparer comparer = null, string message = null)
        {
            if (comparer == null && typeof(TProperty) == typeof(string))
                comparer = StringComparer.Ordinal;

            builder.Validator.Rules.Add(new EqualValidationRule(value, comparer) { ErrorMessage = message });
            return builder;
        }

        public static PropertyValidatorBuilder<T, TProperty> Equal<T, TProperty>(this PropertyValidatorBuilder<T, TProperty> builder, Expression<Func<T, TProperty>> expression, IEqualityComparer comparer = null, string message = null)
        {
            if (comparer == null && typeof(TProperty) == typeof(string))
                comparer = StringComparer.Ordinal;

            builder.Validator.Rules.Add(new EqualValidationRule(expression.Compile(), comparer) { ErrorMessage = message });
            return builder;
        }

        public static PropertyValidatorBuilder<T, TProperty> Must<T, TProperty>(this PropertyValidatorBuilder<T, TProperty> builder, Func<TProperty, bool> predicate, string message = null)
        {
            Check.IfNullThrow(predicate, "Cannot pass a null predicate to Must.");

            return builder.Must((instance, propertyValue, context) => predicate(propertyValue), message);
        }

        public static PropertyValidatorBuilder<T, TProperty> Must<T, TProperty>(this PropertyValidatorBuilder<T, TProperty> builder, Func<T, TProperty, bool> predicate, string message = null)
        {
            Check.IfNullThrow(predicate, "Cannot pass a null predicate to Must.");

            return builder.Must((instance, propertyValue, context) => predicate(instance, propertyValue), message);
        }

        public static PropertyValidatorBuilder<T, TProperty> Must<T, TProperty>(this PropertyValidatorBuilder<T, TProperty> builder, Func<T, TProperty, PropertyValidationContext, bool> predicate, string message = null)
        {
            Check.IfNullThrow(predicate, "Cannot pass a null predicate to Must.");

            builder.Validator.Rules.Add(new PredicateValidationRule((instance, propertyValue, rule, context) => predicate((T)instance, (TProperty)propertyValue, context)) { ErrorMessage = message });
            return builder;
        }

        public static PropertyValidatorBuilder<T, string> Match<T>(this PropertyValidatorBuilder<T, string> builder, Func<T, string> expression, RegexOptions options = RegexOptions.None, string message = null)
        {
            builder.Validator.Rules.Add(new RegularExpressionValidationRule(o => expression((T)o), options) { ErrorMessage = message });
            return builder;
        }

        public static PropertyValidatorBuilder<T, string> Match<T>(this PropertyValidatorBuilder<T, string> builder, string pattern, RegexOptions options = RegexOptions.None, string message = null)
        {
            builder.Validator.Rules.Add(new RegularExpressionValidationRule(pattern, options) { ErrorMessage = message });
            return builder;
        }

        public static PropertyValidatorBuilder<T, TProperty> DataAnnotation<T, TProperty>(this PropertyValidatorBuilder<T, TProperty> builder, string message = null)
        {
            var attributes = builder.Validator.Descriptor.PropertyInfo.GetCustomAttributes<ValidationAttribute>(true);
            builder.Validator.Rules.Add(new DataAnnotationRule(attributes) { ErrorMessage = message });
            return builder;
        }

        public static PropertyValidatorBuilder<T, TProperty> DataAnnotation<T, TProperty>(this PropertyValidatorBuilder<T, TProperty> builder, ValidationAttribute attribute, string message = null)
        {
            builder.Validator.Rules.Add(new DataAnnotationRule(attribute) { ErrorMessage = message });
            return builder;
        }

        public static PropertyValidatorBuilder<T, string> Email<T>(this PropertyValidatorBuilder<T, string> builder, string message = null)
        {
            builder.Validator.Rules.Add(new EmailValidationRule() { ErrorMessage = message });
            return builder;
        }

        public static PropertyValidatorBuilder<T, string> Length<T>(this PropertyValidatorBuilder<T, string> builder, int min, int max, string message = null)
        {
            builder.Validator.Rules.Add(new LengthValidationRule(min, max) { ErrorMessage = message });
            return builder;
        }

        public static PropertyValidatorBuilder<T, string> MinimumLength<T>(this PropertyValidatorBuilder<T, string> builder, int minimumLength, string message = null)
        {
            builder.Validator.Rules.Add(new MinimumLengthValidationRule(minimumLength) { ErrorMessage = message });
            return builder;
        }

        public static PropertyValidatorBuilder<T, string> MaximumLength<T>(this PropertyValidatorBuilder<T, string> builder, int maximumLength, string message = null)
        {
            builder.Validator.Rules.Add(new MaximumLengthValidationRule(maximumLength) { ErrorMessage = message });
            return builder;
        }

        private static PropertyValidatorBuilder<T, TProperty> CreateChildBuilder<T, TProperty>(this PropertyValidatorBuilder<T, TProperty> builder)
        {
            var validator = new PropertyValidator(builder.Validator.Descriptor);
            return new PropertyValidatorBuilder<T, TProperty>(validator);
        }

        public static PropertyValidatorBuilder<T, TProperty> Range<T, TProperty>(this PropertyValidatorBuilder<T, TProperty> builder, TProperty from, TProperty to, string message = null) where TProperty : IComparable<TProperty>, IComparable
        {
            builder.Validator.Rules.Add(new RangeValidationRule(from, to) { ErrorMessage = message });
            return builder;
        }

        public static PropertyValidatorBuilder<T, TProperty?> Range<T, TProperty>(this PropertyValidatorBuilder<T, TProperty?> builder, TProperty from, TProperty to, string message = null) where TProperty : struct, IComparable<TProperty>, IComparable
        {
            builder.Validator.Rules.Add(new RangeValidationRule(from, to) { ErrorMessage = message });
            return builder;
        }
    }
}