using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EasyValidation.Rules
{
    public class EqualValidationRule : ValidationRule
    {
        private readonly Delegate _delegate;

        public EqualValidationRule(object comparisonValue, IEqualityComparer comparer = null)
        {
            ComparisonValue = comparisonValue;
            Comparer = comparer;
        }

        public EqualValidationRule(Delegate @delegate, IEqualityComparer comparer = null)
        {
            _delegate = @delegate;
            Comparer = comparer;
        }

        public IEqualityComparer Comparer { get; }

        public object ComparisonValue { get; }

        protected override string DefaultErrorMessage => "'{PropertyName}' 不能和 '{ComparisonValue}' 相等。";

        public override bool IsValid(PropertyValidationContext context)
        {
            var value = GetComparisonValue(context);
            if(!Compare(context.PropertyValue, value))
            {
                context.FormattedArguments.Add("ComparisonValue", value);
                return false;
            }
            return true;
        }

        private bool Compare(object propertyValue, object value)
        {
            if (Comparer != null)
                return Comparer.Equals(value, propertyValue);

            return Equals(value, propertyValue);
        }

        private object GetComparisonValue(PropertyValidationContext context)
        {
            if (_delegate != null)
                return _delegate.DynamicInvoke(context.Instance);

            return ComparisonValue;
        }
    }
}
